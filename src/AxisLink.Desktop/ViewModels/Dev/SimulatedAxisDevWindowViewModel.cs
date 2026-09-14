using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AxisLink.Desktop.ViewModels.Dev
{
    public record TrafficLogEntry(string Timestamp, string Direction, string Message, string ColorHex);

    public partial class SimulatedAxisDevWindowViewModel : ViewModelBase, IDisposable
    {
        private TcpListener? _listener;
        private CancellationTokenSource? _cts;
        private Task? _serverTask;
        private readonly System.Timers.Timer _physicsTimer;

        // Kinematic Target Registers (stride mapped to Channel 1)
        private int _rawTargetPos = 0;
        private int _rawTargetVel = 2000; // default 1 rev/sec
        private int _rawTargetAccel = 4000;
        private int _rawTargetDecel = 4000;

        // Kinematic Feedback Registers
        private double _simulatedPos = 0.0;
        private double _simulatedVel = 0.0;

        // Drive Status & Control Flags
        private bool _isBusy = false;
        private bool _isFaulted = false;
        private bool _hasHardLimitHigh = false;
        private bool _hasHardLimitLow = false;

        public ObservableCollection<TrafficLogEntry> TrafficLogs { get; } = new();

        [ObservableProperty] private string _connectionStatus = "Listening on 127.0.0.1:5020";
        [ObservableProperty] private bool _isClientConnected = false;

        // UI Telemetry Readouts
        [ObservableProperty] private double _currentPosition;
        [ObservableProperty] private double _currentVelocity;
        [ObservableProperty] private int _targetPosition;
        [ObservableProperty] private int _targetVelocity;
        [ObservableProperty] private bool _busyFlag;
        [ObservableProperty] private bool _faultFlag;

        // Simulation Tuning / Overrides
        [ObservableProperty] private int _artificialLatencyMs = 0;

        public SimulatedAxisDevWindowViewModel()
        {
            // Physics loop running at 50 Hz (20ms)
            _physicsTimer = new System.Timers.Timer(20);
            _physicsTimer.Elapsed += (s, e) => UpdateKinematics(0.02);

            StartMockServer();
        }

        private void StartMockServer()
        {
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Loopback, 5020);
            _listener.Start();
            _physicsTimer.Start();

            _serverTask = Task.Run(() => ServerLoopAsync(_cts.Token));
            Log("SYS", "Virtual Modbus TCP Server initialized on 127.0.0.1:5020", "#4EC9B0");
        }

        private async Task ServerLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var client = await _listener!.AcceptTcpClientAsync(token);
                    _ = Task.Run(() => HandleClientAsync(client, token), token);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Log("ERR", $"Listener error: {ex.Message}", "#F44747");
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            Dispatcher.UIThread.Post(() =>
            {
                IsClientConnected = true;
                ConnectionStatus = "Connected to AxisLink Core";
            });
            Log("CONN", "Client session established from AxisLink host.", "#4EC9B0");

            using var stream = client.GetStream();
            byte[] buffer = new byte[260];

            try
            {
                while (!token.IsCancellationRequested && client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0) break;

                    if (ArtificialLatencyMs > 0)
                        await Task.Delay(ArtificialLatencyMs, token);

                    // Decode basic Modbus TCP Frame Header (MBAP: Transaction ID, Protocol, Length, Unit ID, Function Code)
                    if (bytesRead >= 8)
                    {
                        ushort transactionId = (ushort)((buffer[0] << 8) | buffer[1]);
                        byte functionCode = buffer[7];

                        switch (functionCode)
                        {
                            case 0x01: // Read Coils
                            case 0x02: // Read Discrete Inputs
                                HandleReadCoils(stream, buffer, transactionId, functionCode);
                                break;

                            case 0x03: // Read Holding Registers
                            case 0x04: // Read Input Registers
                                HandleReadRegisters(stream, buffer, transactionId, functionCode);
                                break;

                            case 0x05: // Write Single Coil
                                HandleWriteSingleCoil(stream, buffer, transactionId);
                                break;

                            case 0x06: // Write Single Register
                            case 0x10: // Write Multiple Registers
                                HandleWriteRegisters(stream, buffer, transactionId, functionCode);
                                break;

                            default:
                                Log("WARN", $"Unhandled FC 0x{functionCode:X2}", "#CE9178");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Log("ERR", $"Socket error: {ex.Message}", "#F44747");
            }
            finally
            {
                client.Close();
                Dispatcher.UIThread.Post(() =>
                {
                    IsClientConnected = false;
                    ConnectionStatus = "Listening on 127.0.0.1:5020 (Client Disconnected)";
                });
                Log("DISC", "Client disconnected.", "#CCA700");
            }
        }

        // --- Modbus Decode Routines ---

        private void HandleWriteRegisters(NetworkStream stream, byte[] buffer, ushort transactionId, byte fc)
        {
            ushort regAddress = (ushort)((buffer[8] << 8) | buffer[9]);

            if (fc == 0x10) // Write Multiple
            {
                ushort regCount = (ushort)((buffer[10] << 8) | buffer[11]);
                // Target Pos: Registers 40001 & 40002 (0-indexed address 0 & 1)
                if (regAddress == 0 && regCount >= 2)
                {
                    _rawTargetPos = (buffer[13] << 24) | (buffer[14] << 16) | (buffer[15] << 8) | buffer[16];
                    Dispatcher.UIThread.Post(() => TargetPosition = _rawTargetPos);
                    Log("RX", $"[FC16] Set Target Pos -> {_rawTargetPos} pulses", "#569CD6");
                }
                // Target Vel: Registers 40003 & 40004 (address 2 & 3)
                else if (regAddress == 2 && regCount >= 2)
                {
                    _rawTargetVel = (buffer[13] << 24) | (buffer[14] << 16) | (buffer[15] << 8) | buffer[16];
                    Dispatcher.UIThread.Post(() => TargetVelocity = _rawTargetVel);
                    Log("RX", $"[FC16] Set Target Vel -> {_rawTargetVel} pulses/s", "#569CD6");
                }

                // Send standard FC16 ACK
                byte[] response = new byte[] { (byte)(transactionId >> 8), (byte)transactionId, 0, 0, 0, 6, 1, 0x10, buffer[8], buffer[9], buffer[10], buffer[11] };
                stream.Write(response, 0, response.Length);
            }
        }

        private void HandleWriteSingleCoil(NetworkStream stream, byte[] buffer, ushort transactionId)
        {
            ushort coilAddr = (ushort)((buffer[8] << 8) | buffer[9]);
            bool state = buffer[10] == 0xFF;

            // Address 1: go_cue (Coil 2)
            if (coilAddr == 1 && state)
            {
                Log("RX", "[FC05] Cue GO Pulse Received! Ramping...", "#DCDCAA");
                _isBusy = true;
            }
            // Address 3: stop (Coil 4)
            else if (coilAddr == 3 && state)
            {
                Log("RX", "[FC05] STOP Pulse Received! Aborting move.", "#F44747");
                _isBusy = false;
                _simulatedVel = 0;
            }

            // Echo back request as standard FC05 ACK
            stream.Write(buffer, 0, 12);
        }

        private void HandleReadRegisters(NetworkStream stream, byte[] buffer, ushort transactionId, byte fc)
        {
            ushort regAddress = (ushort)((buffer[8] << 8) | buffer[9]);
            ushort regCount = (ushort)((buffer[10] << 8) | buffer[11]);

            byte byteCount = (byte)(regCount * 2);
            byte[] response = new byte[9 + byteCount];
            response[0] = (byte)(transactionId >> 8);
            response[1] = (byte)transactionId;
            response[4] = (byte)((3 + byteCount) >> 8);
            response[5] = (byte)(3 + byteCount);
            response[6] = 1; // Unit ID
            response[7] = fc;
            response[8] = byteCount;

            // Address 10 & 11: actual_pos (40011)
            int currentSteps = (int)Math.Round(_simulatedPos);
            response[9] = (byte)(currentSteps >> 24);
            response[10] = (byte)(currentSteps >> 16);
            response[11] = (byte)(currentSteps >> 8);
            response[12] = (byte)currentSteps;

            stream.Write(response, 0, response.Length);
        }

        private void HandleReadCoils(NetworkStream stream, byte[] buffer, ushort transactionId, byte fc)
        {
            byte statusByte = 0;
            if (_isBusy) statusByte |= (1 << 2);    // busy (Coil 11)
            if (_isFaulted) statusByte |= (1 << 3); // fault (Coil 12)

            byte[] response = new byte[] { (byte)(transactionId >> 8), (byte)transactionId, 0, 0, 0, 4, 1, fc, 1, statusByte };
            stream.Write(response, 0, response.Length);
        }

        // --- Physics Kinematic Step ---

        private void UpdateKinematics(double dt)
        {
            if (_isBusy && !_isFaulted)
            {
                double distanceToTarget = _rawTargetPos - _simulatedPos;
                double direction = Math.Sign(distanceToTarget);

                if (Math.Abs(distanceToTarget) <= Math.Abs(_rawTargetVel * dt))
                {
                    _simulatedPos = _rawTargetPos;
                    _simulatedVel = 0;
                    _isBusy = false;
                    Log("PHYS", $"Target reached at position {_simulatedPos:F0}", "#B5CEA8");
                }
                else
                {
                    _simulatedVel = direction * Math.Abs(_rawTargetVel);
                    _simulatedPos += _simulatedVel * dt;
                }
            }
            else if (!_isBusy)
            {
                _simulatedVel = 0;
            }

            Dispatcher.UIThread.Post(() =>
            {
                CurrentPosition = _simulatedPos;
                CurrentVelocity = _simulatedVel;
                BusyFlag = _isBusy;
                FaultFlag = _isFaulted;
            });
        }

        // --- Simulation Overrides ---

        [RelayCommand]
        private void ToggleFault()
        {
            _isFaulted = !_isFaulted;
            if (_isFaulted)
            {
                _isBusy = false;
                _simulatedVel = 0;
                Log("SIM", "FAULT INJECTED: Drive tripped offline!", "#F44747");
            }
            else
            {
                Log("SIM", "Fault cleared.", "#4EC9B0");
            }
        }

        [RelayCommand]
        private void ResetPositionZero()
        {
            _simulatedPos = 0;
            _simulatedVel = 0;
            _isBusy = false;
            Log("SIM", "Position manually forced to 0 pulses.", "#DCDCAA");
        }

        [RelayCommand]
        private void ClearLogs() => TrafficLogs.Clear();

        private void Log(string direction, string message, string hexColor)
        {
            Dispatcher.UIThread.Post(() =>
            {
                TrafficLogs.Add(new TrafficLogEntry(
                    DateTime.Now.ToString("HH:mm:ss.fff"),
                    direction,
                    message,
                    hexColor
                ));

                while (TrafficLogs.Count > 200)
                    TrafficLogs.RemoveAt(0);
            });
        }

        public void Dispose()
        {
            _physicsTimer.Stop();
            _physicsTimer.Dispose();
            _cts?.Cancel();
            _listener?.Stop();
        }
    }
}