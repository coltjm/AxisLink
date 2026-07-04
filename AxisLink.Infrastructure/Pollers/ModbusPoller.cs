using AxisLink.Core.Models.Extended;
using AxisLink.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AxisLink.Infrastructure.Background
{
    public class ModbusPoller : IPoller
    {

        public event Action<ExtendedAxis>? PollCompleted;
        public event Action<Exception>? PollFailed;
        public bool IsPolling { get; set;} = false;

        public void Start(IEnumerable<ExtendedAxis> axes, TimeSpan interval)
        {
            IsPolling = true;
            foreach (ExtendedAxis axis in axes) {
                Poll(axis);
            }
            throw new NotImplementedException();
        }
        public void Stop()
        {
            IsPolling = false;
            throw new NotImplementedException();
        }

        protected async Task Poll(ExtendedAxis axis) 
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
