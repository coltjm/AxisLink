using AxisLink.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Show
{
    public enum SensorTypes
    {
        // Lowest (most negative) allowable positive position - Recommend having an additional sensor beyond this one the is wired to shut off motor in PLC
        [XmlEnum("low_limit")] LowLimit,
        // Highest (most positive) allowable positive position - Recommend having an additional sensor beyond this one the is wired to shut off motor in PLC
        [XmlEnum("high_limit")] HighLimit,
        [XmlEnum("home")] Home,
        // Inverted logic to normal limit - safe position when reached
        [XmlEnum("interlock")] Interlock,
        // Nominative location - not for safety
        [XmlEnum("position_reference")] PositionReference
    }

    [XmlType("sensor")]
    public class Sensor 
    {
        // Parameterless constructor for xml serialization and deserialization
        public Sensor() { }
        [XmlAttribute("id")]
        public required int Id { get; set; }
        // Sensors might be connected to a different plc than associated axis - I dont recommend but software will allow
        [XmlElement("controller_id")]
        public int? ControllerId { get; set; }

        [XmlElement("type")]
        public SensorTypes SensorType { get; set; }

        // Is the sensor a normally closed sensor?
        [XmlElement("nc")]
        public bool NC {  get; set; }

        [XmlElement("hardware_config")]
        public SensorHardwareConfig? Config { get; set; }
    }
}
