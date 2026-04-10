using KinetiCUE.Core.Models.KQ;
using KinetiCUE.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Infrastructure.Background
{
    public class ModbusPoller : IPoller
    {

        private readonly IMotionService _service;
        private readonly IEnumerable<KQAxis> _axes;

        public async Task RunAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                // The Poller triggers the capability
                await _service.UpdateAllStatesAsync(_axes);

                await Task.Delay(50, ct); // 20Hz
            }
        }
    }
}
