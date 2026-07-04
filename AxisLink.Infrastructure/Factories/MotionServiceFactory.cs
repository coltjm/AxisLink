using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Extended;
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
        public IMotionService CreateService(ConnectionConfig config)
        {
            return config switch
            {
                ModbusConfig modbus => new ModbusMotionService(modbus.IpAddress, modbus.Port),
                // BeckhoffConfig beckhoff => new BeckhoffAdsService(beckhoff.AmsNetId),
                _ => throw new NotSupportedException($"Unknown config type: {config.GetType().Name}")
            };
        }
    }
}
