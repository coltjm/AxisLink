using AxisLink.Core.Management;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Show;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    public interface IMotionServiceFactory
    {
        // Creates motion service based on the type of connection
        IMotionService CreateService(Controller controller, MotionManager? motionManager = null);
    }
}
