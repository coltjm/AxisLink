using AxisLink.Core.Models.Extended;
using AxisLink.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Infrastructure.Services
{
    public class ModbusMotionService : IMotionService
    {
        public bool IsConnected => throw new NotImplementedException();
        private string _ipAddress;
        private string _port;

        public ModbusMotionService(string ipAddress, string port)
        {
            _ipAddress = ipAddress;
            _port = port;
            throw new NotImplementedException();
        }

        public async Task ConnectAsync()
        {
            // Conect using the provided IP address and port
            throw new NotImplementedException();
        }

        public Task ConnectAsync(string ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public void ExecuteMove()
        {
            throw new NotImplementedException();
        }

        public Task ExecuteMoveAsync(ExtendedAxis axis, float targetPosition)
        {
            throw new NotImplementedException();
        }

        public Task JogAsync(ExtendedAxis axis, float velocity)
        {
            throw new NotImplementedException();
        }

        public Task StopAxisAsync(ExtendedAxis axis)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAllStatesAsync(IEnumerable<ExtendedAxis> axes)
        {
            // Read all 
            throw new NotImplementedException();
        }

        public void UpdateState()
        {
            throw new NotImplementedException();
        }

    }
}
