using KinetiCUE.Core.Models.KQ;
using KinetiCUE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KinetiCUE.Infrastructure.Background
{
    public class ModbusPoller : IPoller
    {

        public event Action<KQAxis>? PollCompleted;
        public event Action<Exception>? PollFailed;
        public bool IsPolling { get; set;} = false;

        public void Start(IEnumerable<KQAxis> axes, TimeSpan interval)
        {
            IsPolling = true;
            foreach (KQAxis axis in axes) {
                Poll(axis);
            }
            throw new NotImplementedException();
        }
        public void Stop()
        {
            IsPolling = false;
            throw new NotImplementedException();
        }

        protected async Task Poll(KQAxis axis) 
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
