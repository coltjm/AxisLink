using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Show;
using AxisLink.Core.Models.Sprockets;
using AxisLink.Infrastructure.Services;
using AxisLink.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Infrastructure.Factories
{
    public class MotionServiceFactory : IMotionServiceFactory
    {
        // Takes motion type and creates relevant services
        public IMotionService CreateService(Controller controller)
        {
            return controller.Protocol switch
            {
                TransportProtocol.ModbusTcp => new ModbusMotionService(controller.IpAddress, controller.Port.ToString()),
                // BeckhoffConfig beckhoff => new BeckhoffAdsService(beckhoff.AmsNetId),
                _ => throw new NotSupportedException($"Unknown config type: {controller.Protocol.ToString()}")
            };
        }
    }
}
