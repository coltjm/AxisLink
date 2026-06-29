using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Configs
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
        public string? IpAddress { get; set; }
        public string? Port { get; set; }

        public string? EStopAddress { get; set; }

    }

}
