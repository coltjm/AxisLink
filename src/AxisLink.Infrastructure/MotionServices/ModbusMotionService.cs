using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using AxisLink.Core.Models.Sprockets;
using AxisLink.Infrastructure.Sprockets;
using NModbus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AxisLink.Core.Utilities;

namespace AxisLink.Infrastructure.Services
{

    public record ResolvedModbusTarget(ushort Address, ModbusDataType DataType);

    public class ResolvedAxisContext
    {
        public Axis Axis { get; }
        public Sprocket Sprocket { get; }
        public Dictionary<string, ResolvedModbusTarget> Targets { get; } = new();

        public ResolvedAxisContext(Axis axis, Sprocket sprocket)
        {
            Axis = axis;
            Sprocket = sprocket;

            int channel = axis.Channel.Value > 0 ? axis.Channel.Value : 1;

            foreach (var (name, def) in sprocket.AddressAliases)
            {
                // Evaluate formula replacing $channel
                IReadOnlyDictionary<string, double> substitutionDict = new Dictionary<string, double>
                {
                    ["$channel"] = channel
                };
                int address = ExpressionEvaluator.EvaluateInt(def.Address, substitutionDict);
                Targets[name] = new ResolvedModbusTarget( (ushort)address, def.ModbusDataType );
            }
        }
    }

    public class ModbusCommand
    {
        public ushort Address { get; set; }
        public ModbusDataType DataType { get; set; }
        public double Value { get; set; }
        public ModbusCommand(ushort address, ModbusDataType dataType, double value)
        {
            Address = address; 
            DataType = dataType; 
            Value = value;
        }

    }

    public class ModbusMotionService : IMotionService
    {
        public bool IsConnected { get; private set; } = false;
        private string _ipAddress;
        private int _port;
        private byte _slaveId;
        private TcpClient _tcpClient;
        private ModbusFactory _factory;
        private IModbusMaster _modbusMaster;
        private readonly float heartbeatTimeMs = 500;
        private readonly ISprocketProvider _sprocketProvider;
        private readonly MotionManager _motionManager;
        // Process all aliases when the axis is added to this controller and store it
        private Dictionary<int, ResolvedAxisContext> resolvedAxes;

        public ModbusMotionService(string ipAddress, int port, byte slaveId, MotionManager motionManager)
        {
            _ipAddress = ipAddress;
            _port = port;
            _factory = new ModbusFactory();
            _slaveId = slaveId;
            resolvedAxes = new Dictionary<int, ResolvedAxisContext>();
            _motionManager = motionManager;
            _sprocketProvider = _motionManager._sprocketProvider;
        }

        public async Task ConnectAsync()
        {
            _tcpClient = new TcpClient(_ipAddress, _port);
            _modbusMaster = _factory.CreateMaster(_tcpClient);
            IsConnected = true;
            //throw new NotImplementedException();
        }

        public Task DisconnectAsync()
        {
            _tcpClient.Close();
            IsConnected = false;
            throw new NotImplementedException();
        }

        public Task ExecuteMoveAsync(Axis axis, float targetPosition, float velocity, float acceleration, float deceleration, CancellationToken cancellationToken = default)
        {
            
            try
            {
                if (_modbusMaster == null)
                {
                    throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
                }
            }
            catch (Exception ex) 
            {
                
            }
            finally
            {

            }
            
            
            //Sprocket sprocket = GetSprocketById(axis.SprocketId);
            throw new NotImplementedException();
        }

        public async Task JogAsync(Axis axis, float velocity, float acceleration, float deceleration, CancellationToken cancellationToken = default)
        {
            if (_modbusMaster == null)
            {
                throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
            }
            ResolvedAxisContext resolvedAxis = resolvedAxes[axis.Id];
            var sprocket = resolvedAxis.Sprocket;
            float effectiveScale = axis.DriveScaleFactor ?? sprocket.DefaultDriveScaleFactor;
            var substitutionDict = new Dictionary<string, double>
            {
                ["$velocity"] = velocity * effectiveScale,
                ["$acceleration"] = acceleration * effectiveScale,
                ["$deceleration"] = deceleration * effectiveScale,
                ["$accel"] = acceleration * effectiveScale,
                ["$decel"] = deceleration * effectiveScale
            };
            try
            { 
                await ExecuteSequenceAsync(sprocket.RunJogSequence, resolvedAxis, substitutionDict, cancellationToken);

            }
            catch (OperationCanceledException)
            {
                await ExecuteSequenceAsync(sprocket.StopSequence, resolvedAxis, substitutionDict, CancellationToken.None);
                throw;
            }
            catch (Exception ex)
            {
                // log exception and write stop bits
            }
            finally
            {
                // handle any cleanup
            }
        }

        private static List<List<ModbusCommand>> GroupConsecutive(List<ModbusCommand> commands, int spacing = 1)
        {
            if (commands.Count > 0)
            {
                commands.Sort(delegate (ModbusCommand x, ModbusCommand y)
                {
                    return x.Address.CompareTo(y.Address);
                });
                List<List<ModbusCommand>> groupedCommands = new List<List<ModbusCommand>>();
                // Break the cluster into consecutive segments
                List<ModbusCommand> curCoilGroup = new();
                curCoilGroup.Add(commands[0]);
                for (int j = 1; j < commands.Count; j++)
                {
                    if (commands[j].Address == commands[j - 1].Address + spacing)
                    {
                        curCoilGroup.Add(commands[j]);
                    }
                    else
                    {
                        groupedCommands.Add(curCoilGroup);
                        curCoilGroup = new List<ModbusCommand>();
                        curCoilGroup.Add(commands[j]);
                    }
                }
                if (curCoilGroup.Count > 0)
                {
                    groupedCommands.Add(curCoilGroup);
                    
                }
                return groupedCommands;
            }
            return new List<List<ModbusCommand>>();
        }

        private async Task ExecuteSequenceAsync(
            IEnumerable<SprocketStep> steps,
            ResolvedAxisContext resolvedAxis,
            IReadOnlyDictionary<string, double>? substitutionDict,
            CancellationToken cancellationToken)
        {
            // Dictionary in the form of (Data type, cluster group number) => List of commands
            Dictionary<(ModbusDataType, int), List<ModbusCommand>> commandClusters = new();
            // delays are numbered such that the delay of each index comes BEFORE the group of that index
            // ie run order is cluster 0, delay 1, then cluster 1
            Dictionary<int, int> delays = new();
            int currentClusterNumber = 0;
            commandClusters[(ModbusDataType.Coil, 0)] = new();
            commandClusters[(ModbusDataType.Register16, 0)] = new();
            commandClusters[(ModbusDataType.Register32, 0)] = new();
            // Create write clusters
            foreach (SprocketStep step in steps)
            {
                if (step.DelayMs > 0)
                {
                    // start new cluster
                    currentClusterNumber++;
                    commandClusters[(ModbusDataType.Coil, currentClusterNumber)] = new();
                    commandClusters[(ModbusDataType.Register16, currentClusterNumber)] = new();
                    commandClusters[(ModbusDataType.Register32, currentClusterNumber)] = new();
                    // Add delay to delays at next index
                    delays[currentClusterNumber] = step.DelayMs;
                }
                // We can have steps that are just delays
                if (string.IsNullOrEmpty(step.Target))
                    continue;
                // Get the modbus address and type from the target string
                // Resolved axis returns the target already processed according to channel number
                if (!resolvedAxis.Targets.TryGetValue(step.Target, out var target))
                    throw new KeyNotFoundException($"Target '{step.Target}' not defined in Sprocket registers.");
                double value = string.IsNullOrEmpty(step.Value)
                    ? 0
                    : ExpressionEvaluator.Evaluate(step.Value, substitutionDict);
                commandClusters[(target.DataType, currentClusterNumber)].Add(
                    new ModbusCommand(target.Address, target.DataType, value));
                if (step.PulseMs > 0)
                {
                    // Create new clusters
                    currentClusterNumber++;
                    commandClusters[(ModbusDataType.Coil, currentClusterNumber)] = new();
                    commandClusters[(ModbusDataType.Register16, currentClusterNumber)] = new();
                    commandClusters[(ModbusDataType.Register32, currentClusterNumber)] = new();
                    // Add pulse delay to delays at the next index
                    delays[currentClusterNumber] = step.PulseMs;
                    // Add pulse low value to next cluster
                    double pulseLowValue = string.IsNullOrEmpty(step.PulseLowValue)
                        ? 0
                        : ExpressionEvaluator.Evaluate(step.PulseLowValue, substitutionDict);
                    // Send command to same address but with different value
                    commandClusters[(target.DataType, currentClusterNumber)].Add(
                        new ModbusCommand(target.Address, target.DataType, pulseLowValue));
                }
            }
            for (int i = 0; i <= currentClusterNumber; i++)
            {
                // COILS
                List<ModbusCommand> coilCommands = commandClusters[(ModbusDataType.Coil, i)];
                List<List<ModbusCommand>> groupedCoilCommands = GroupConsecutive(coilCommands, 1);
                foreach (List<ModbusCommand> commandGroup in groupedCoilCommands)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await _modbusMaster.WriteMultipleCoilsAsync(
                        _slaveId,
                        commandGroup.First().Address,
                        commandGroup.Select(com => Convert.ToBoolean(com.Value)).
                        ToArray()).
                        WaitAsync(cancellationToken);
                }
                // 16 BIT REGISTERS
                List<ModbusCommand> Reg16Commands = commandClusters[(ModbusDataType.Register16, i)];
                List<List<ModbusCommand>> groupedReg16Commands = GroupConsecutive(Reg16Commands, 1);
                foreach (List<ModbusCommand> commandGroup in groupedReg16Commands)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await _modbusMaster.WriteMultipleRegistersAsync(
                        _slaveId,
                        commandGroup.First().Address,
                        commandGroup.Select(com => (ushort)com.Value).
                        ToArray()).
                        WaitAsync(cancellationToken);
                }
                // 32 BIT REGISTERS
                List<ModbusCommand> Reg32Commands = commandClusters[(ModbusDataType.Register32, i)];
                List<List<ModbusCommand>> groupedReg32Commands = GroupConsecutive(Reg32Commands, 2);
                foreach (List<ModbusCommand> commandGroup in groupedReg32Commands)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    // for 32 bit registers, we need to split into twice as many ushorts defaulting to little endian but maybe with the option to define it in sprocket
                    float[] floatValueArray = commandGroup.Select(com => (float)com.Value).ToArray();
                    ushort[] valueUShorts = new ushort[commandGroup.Count * 2];
                    for (int j = 0; j < commandGroup.Count; j++)
                    {
                        int intRepresentation = BitConverter.SingleToInt32Bits(floatValueArray[j]);
                        ushort low = (ushort)(intRepresentation & 0x0000FFFF);
                        ushort high = (ushort)((intRepresentation & 0xFFFF0000) >> 16);

                        valueUShorts[j * 2] = low;
                        valueUShorts[j * 2 + 1] = high;
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    await _modbusMaster.WriteMultipleRegistersAsync(
                        _slaveId,
                        commandGroup.First().Address,
                        valueUShorts).
                        WaitAsync(cancellationToken);
                }

                // call delays
                if (delays.TryGetValue(i, out int delayMs) && delayMs > 0)
                {
                    await Task.Delay(delayMs, cancellationToken);
                }
            }
        }

        public Task StopAxisAsync(Axis axis)
        {
            if (_modbusMaster == null)
            {
                throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
            }
            throw new NotImplementedException();
        }

        public Task UpdateAllStatesAsync(IEnumerable<Axis> axes)
        {
            if (_modbusMaster == null)
            {
                throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
            }
            // Read all 
            throw new NotImplementedException();
        }

        // TODO make async
        public void UpdateState()
        {
            if (_modbusMaster == null)
            {
                throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
            }
            throw new NotImplementedException();
        }

        public void Heartbeat()
        {
            if (_modbusMaster == null)
            {
                throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
            }
            // Read heartbeat signal then sent new
            // On plc when the heartbeat coil is set high, the plc will reset a timer. If the timer expires, the plc will stop all motion and set an alarm. The heartbeat signal is typically a coil that is set high for a short period of time and then set low for a short period of time. The plc will reset the timer when the coil is set high.

            // I want heartbeat handled on both side, if the plc does not receive a heartbeat signal within a certain time frame, it will stop all motion and set an alarm.
            // If the motion service does not receive a heartbeat signal from the plc within a certain time frame, it will stop all motion and set an alarm.
            // If the heartbeat signal is not received within a certain time frame, the motion service will stop all motion and set an alarm.
            // The heartbeat signal can be sent using a Modbus write command to set the coil high and then low.
            throw new NotImplementedException();

        }
    
        public void AddAxis(Axis axis)
        {
            ResolvedAxisContext newAxisContext = new ResolvedAxisContext(axis, _sprocketProvider.GetSprocketById(axis.SprocketId));
            resolvedAxes[axis.Id] = newAxisContext;
        }

        public void RemoveAxis(Axis axis) 
        {
            resolvedAxes.Remove(axis.Id);
        }
    
        
    }
}
