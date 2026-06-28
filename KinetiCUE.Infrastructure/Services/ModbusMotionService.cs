using KinetiCUE.Core.Models.KQ;
using KinetiCUE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Infrastructure.Services
{
    public class ModbusMotionService : IMotionService
    {
        public bool IsConnected => throw new NotImplementedException();

        public async Task ConnectAsync()
        {
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

        public Task ExecuteMoveAsync(KQAxis axis, float targetPosition)
        {
            throw new NotImplementedException();
        }

        public Task JogAsync(KQAxis axis, float velocity)
        {
            throw new NotImplementedException();
        }

        public Task StopAxisAsync(KQAxis axis)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAllStatesAsync(IEnumerable<KQAxis> axes)
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
