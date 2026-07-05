using AxisLink.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    public interface IMotionServiceFactory
    {
        IMotionService CreateService(ConnectionConfig config);
    }
}
