using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Standard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Extended
{
    public enum HardwareType
    {
        [XmlEnum("modbus")] Modbus
    }

    [XmlType("kq_axis")]
    public class ExtendedAxis : StandardAxis
    {
        // Parameterless constructor for xml serialization and deserialization
        public ExtendedAxis() { }
        [XmlElement("controller_id")]
        public int? ControllerId { get; set; }

        [XmlElement("steps_per_rev")]
        public int? StepsPerRevolution { get; set; }

        // Distance the output moves per revolution of motor (circumference of output device typically), in mm or degrees
        // In the future, can auto calc this for some configurations
        [XmlElement("dist_per_rev")]
        public float? DistancePerRevolution { get; set; }

        [XmlArray("sensors")]
        [XmlArrayItem("sensor")]
        public List<ExtendedSensor> Sensors { get; set; } = new();

        // Type of interface. Currently only modbus is supported
        [XmlElement("hardware_type")]
        public HardwareType? HardwareType { get; set; }

        // Config is polymorphic and can function for different motor/connection types
        [XmlElement("hardware_config")]
        public AxisHardwareConfig? Config { get; set; }

        [XmlIgnore]
        public bool? IsEnabled { get; set; }

        [XmlIgnore]
        public bool? HasAlarm { get; set; }

        // in mm or degrees
        [XmlIgnore]
        public float? CurrentPos { get; set; }

        // in mm/s or degree/s
        [XmlIgnore]
        public float? CurrentVel { get; set; }


    }
}
