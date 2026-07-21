using AxisLink.Core.Models.Show;
using AxisLink.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AxisLink.Infrastructure.Pollers
{
    public class ModbusPoller : IPoller
    {

        public event Action<Axis>? PollCompleted;
        public event Action<Exception>? PollFailed;
        public bool IsPolling { get; set;} = false;

        public void Start(IEnumerable<Axis> axes, TimeSpan interval)
        {
            IsPolling = true;
            foreach (Axis axis in axes) {
                Poll(axis);
            }
            throw new NotImplementedException();
        }
        public void Stop()
        {
            IsPolling = false;
            throw new NotImplementedException();
        }

        protected async Task Poll(Axis axis) 
        {
            while (IsPolling) {
                try { 
                    Debug.WriteLine("Polling...");
                    PollCompleted?.Invoke(axis);
                }
                catch (Exception e) {
                    PollFailed?.Invoke(e);
                }
            }
        }


    }
}
