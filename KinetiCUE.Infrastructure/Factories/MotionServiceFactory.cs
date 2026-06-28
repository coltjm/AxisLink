using KinetiCUE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using KinetiCUE.Infrastructure.Utils;
using KinetiCUE.Core.Models.KQ;
using KinetiCUE.Infrastructure.Services;

namespace KinetiCUE.Infrastructure.Factories
{
    internal class MotionServiceFactory
    {
        // Takes motion type and creates relevant services
        public IMotionService CreateService(Enums.MotionType motionType, KQAxis axis)
        {
            if(motionType == Enums.MotionType.Modbus)
            {
                return new ModbusMotionService();
            }
            return new SimulatedMotionService();
        }
    }
}
