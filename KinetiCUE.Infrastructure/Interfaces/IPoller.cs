using System;
using System.Collections.Generic;
using System.Text;
using KinetiCUE.Core.Models.KQ;

namespace KinetiCUE.Infrastructure.Interfaces
{
    internal interface IPoller
    {
        bool IsPolling { get; }

        void Start(IEnumerable<KQAxis> axes, TimeSpan interval);
        void Stop();

        event Action<IEnumerable<KQAxis>>? PollCompleted;
        event Action<Exception>? PollFailed;
    }
}
