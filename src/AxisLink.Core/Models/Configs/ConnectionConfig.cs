using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Configs
{
    // TODO should consider serial vs ethernet

    [XmlInclude(typeof(ModbusConfig))]
    // [XmlInclude(typeof(OTHERConfig))]
    public abstract class ConnectionConfig
    {
        // Common properties for all connection types

        // Parameterless constructor for xml serialization and deserialization
        public ConnectionConfig() { }
    }

    public class ModbusConfig : ConnectionConfig
    {
        // Parameterless constructor for xml serialization and deserialization
        public ModbusConfig() { }

        public ModbusConfig(string? ipAddress, string? port, string? eStopAddress)
        {
            IpAddress = ipAddress;
            Port = port;
            EStopAddress = eStopAddress;
        }

        public string? IpAddress { get; set; }
        public string? Port { get; set; }

        public string? EStopAddress { get; set; }

    }

}
