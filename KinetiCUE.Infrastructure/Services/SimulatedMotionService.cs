using KinetiCUE.Core.Models.KQ;
using KinetiCUE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Infrastructure.Services
{
    public class SimulatedMotionService : IMotionService
    {
        public bool IsConnected => throw new NotImplementedException();

        public Task ConnectAsync(string ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync()
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
            throw new NotImplementedException();
        }
    }
}
