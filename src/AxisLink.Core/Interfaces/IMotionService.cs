using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    // Manages interaction with controllers.
    public interface IMotionService
    {
        // Connect to controller
        Task ConnectAsync();
        // Disconnect from controller
        Task DisconnectAsync();
        // Flag indicating if the service is connected to the controller
        bool IsConnected { get; }

        
        Task UpdateAllStatesAsync(IEnumerable<ExtendedAxis> axes);
        Task ExecuteMoveAsync(ExtendedAxis axis, float targetPosition);
        Task StopAxisAsync(ExtendedAxis axis);
        Task JogAsync(ExtendedAxis axis, float velocity);
    }
}
