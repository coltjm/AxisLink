using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
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

        // Modbus Tcp uses slave ids in addition to IP to identify devices for some header information
        // Stored in Infrastructure layer to keep protocol specific information out of Core layer
        public byte nextModbusTcpSlaveId { get; private set; }
        
        public MotionServiceFactory()
        {
            nextModbusTcpSlaveId = 1;
        }

        // Takes motion type and creates relevant services
        public IMotionService CreateService(Controller controller, MotionManager? motionManager = null)
        {
            return controller.Protocol switch
            {
                TransportProtocol.ModbusTcp => new ModbusMotionService(controller.IpAddress, controller.Port, nextModbusTcpSlaveId++, motionManager),
                //TransportProtocol.AsciiTcp => new AsciiTcpMotionService(controller.IpAddress, controller.Port.ToString()),
                // BeckhoffConfig beckhoff => new BeckhoffAdsService(beckhoff.AmsNetId),
                _ => throw new NotSupportedException($"Unknown config type: {controller.Protocol.ToString()}")
            };

        }
    }
}
