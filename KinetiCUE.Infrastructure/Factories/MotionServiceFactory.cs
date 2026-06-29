using KinetiCUE.Core.Interfaces;
using KinetiCUE.Core.Models.Configs;
using KinetiCUE.Core.Models.KQ;
using KinetiCUE.Infrastructure.Services;
using KinetiCUE.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Infrastructure.Factories
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
