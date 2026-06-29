using KinetiCUE.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Core.Interfaces
{
    public interface IMotionServiceFactory
    {
        IMotionService CreateService(ConnectionConfig config);
    }
}
