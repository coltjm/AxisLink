using KinetiCUE.Core.Models.KQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Infrastructure.Interfaces
{
    public interface IMotionService
    {
        Task ConnectAsync(string ipAddress, int port);
        Task DisconnectAsync();
        bool IsConnected { get; }

        Task UpdateAllStatesAsync(IEnumerable<KQAxis> axes);

        Task ExecuteMoveAsync(KQAxis axis, float targetPosition);
        Task StopAxisAsync(KQAxis axis);
        Task JogAsync(KQAxis axis, float velocity);
    }
}
