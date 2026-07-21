using AxisLink.Core.Models.Show;
using AxisLink.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace AxisLink.Infrastructure.MotionServices
{
    public class SimulatedMotionService : IMotionService
    {
        public bool IsConnected { get; private set; } = false;
        public bool IsJogging { get; private set; } = false;
        private string _ipAddress;
        private string _port;
        public SimulatedMotionService(string ipAddress, string port) 
        {
            _ipAddress = ipAddress;
            _port = port;

        }
        public async Task ConnectAsync()
        {

            // Think about how to mark the connection as complete - depends on where the connect call is made from
            Debug.WriteLine($"Connecting to {_ipAddress} on port {_port}");
            await Task.Delay(500);
            IsConnected = true;
        }

        public async Task DisconnectAsync()
        {
            Debug.WriteLine($"Disconnecting");
            await Task.Delay(500);
            IsConnected = false;
        }

        // needs to include things like expected start, accel, vel, decel, etc.
        public async Task ExecuteMoveAsync(Axis axis, float targetPosition)
        {
            Debug.WriteLine($"Moving Axis {axis.Name} to position: {targetPosition}");
            if (axis.CurrentPos <  targetPosition) 
            {
                while (axis.CurrentPos < targetPosition)
                {
                    axis.CurrentPos += 5;
                    Debug.WriteLine($"Current Position: {axis.CurrentPos}");
                }
            }
            else
            {
                while (axis.CurrentPos > targetPosition)
                {
                    axis.CurrentPos -= 5;
                    Debug.WriteLine($"Current Position: {axis.CurrentPos}");
                }
            }
            Debug.WriteLine($"Axis Move Complete. Axis Position: {axis.CurrentPos}");
        }

        public async Task JogAsync(Axis axis, float velocity)
        {
            Debug.WriteLine($"Jogging Axis {axis.Name} with velocity: {velocity}");
            while (IsJogging)
            {
                axis.CurrentPos += velocity;
                Debug.WriteLine($"Current Position: {axis.CurrentPos}");
                await Task.Delay(250);
            }
            Debug.WriteLine($"Axis Jog Complete. Axis Position: {axis.CurrentPos}");
        }

        public async Task StopAxisAsync(Axis axis)
        {
            IsJogging = false;
            Debug.WriteLine($"Stopping Axis {axis.Name}");
        }

        public async Task UpdateAllStatesAsync(IEnumerable< Axis> axes)
        {
            throw new NotImplementedException();
        }
    }
}
