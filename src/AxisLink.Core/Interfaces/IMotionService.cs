using AxisLink.Core.Models.Show;
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

        
        Task UpdateAllStatesAsync(IEnumerable<Axis> axes);
        Task ExecuteMoveAsync(Axis axis, float targetPosition, float velocity, float acceleration, float deceleration);
        Task StopAxisAsync(Axis axis);
        Task JogAsync(Axis axis, float velocity);
    }
}
