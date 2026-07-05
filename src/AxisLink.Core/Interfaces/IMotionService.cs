using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    // Manages interaction with motors.
    public interface IMotionService
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        bool IsConnected { get; }

        Task UpdateAllStatesAsync(IEnumerable<ExtendedAxis> axes);

        Task ExecuteMoveAsync(ExtendedAxis axis, float targetPosition);
        Task StopAxisAsync(ExtendedAxis axis);
        Task JogAsync(ExtendedAxis axis, float velocity);
    }
}
