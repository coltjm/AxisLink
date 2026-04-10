using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Configs
{
    [XmlInclude(typeof(ModbusSensorConfig))]
    public abstract class SensorHardwareConfig
    {
        // Parameterless constructor for xml serialization and deserialization
        public SensorHardwareConfig() { }
    }

    public class ModbusSensorConfig : SensorHardwareConfig
    {
        // Parameterless constructor for xml serialization and deserialization
        public ModbusSensorConfig() { }
        // X### - INPUT COIL
        [XmlElement("signal_addr")]
        public string? SignalAddress { get; set; }
    }
}
