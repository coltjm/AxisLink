using AxisLink.Core.Models.Show;
using AxisLink.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using NModbus;
using System.Net.Sockets;
using AxisLink.Core.Models.Sprockets;
using AxisLink.Infrastructure.Sprockets;

namespace AxisLink.Infrastructure.Services
{
    public class ModbusMotionService : IMotionService
    {
        public bool IsConnected { get; private set; } = false;
        private string _ipAddress;
        private int _port;
        private int _slaveId;
        private TcpClient _tcpClient;
        private ModbusFactory _factory;
        private IModbusMaster _modbusMaster;
        private readonly float heartbeatTimeMs = 500;

        public ModbusMotionService(string ipAddress, int port, int slaveId)
        {
            _ipAddress = ipAddress;
            _port = port;
            _factory = new ModbusFactory();
            _slaveId = slaveId;
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

        public Task ExecuteMoveAsync(Axis axis, float targetPosition, float velocity, float acceleration, float deceleration)
        {
            if (_modbusMaster == null)
            {
                throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
            }
            // Follow the sequence of steps defined in the sprocket to execute a move command
            //Sprocket sprocket = GetSprocketById(axis.SprocketId);
            throw new NotImplementedException();
        }

        public Task JogAsync(Axis axis, float velocity)
        {
            if (_modbusMaster == null)
            {
                throw new InvalidOperationException("Modbus master is not initialized. Call ConnectAsync first.");
            }
            throw new NotImplementedException();
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
    }
}
