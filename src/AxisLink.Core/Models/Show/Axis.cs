using AxisLink.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Show
{
    // AxisLink supported hardware types
    public enum HardwareType
    {
        [XmlEnum("modbus")] Modbus
    }

    // Location subclass specific to Axes in E1.44-2014 R2024
    public class AxisLocation
    {
        // Parameterless constructor for xml serialization and deserialization
        public AxisLocation() { }
        // See ANSI E1.44-2014 R2024 5.3.1.1 for details on how location is defined
        // x poisition in mm
        [XmlElement("b_x")]
        public int X { get; set; }

        // y position in mm
        [XmlElement("b_y")]
        public int Y { get; set; }
    }

    // Enum for machine types
    public enum AxisType
    {
        // lineset_cs, lineset_ud, point_hoist, rotary, other  (see page 17 of ANSI E1.44-2014 R2024)
        [XmlEnum("lineset_cs")] CrossStageLineSet,
        [XmlEnum("lineset_ud")] UpDownLineSet,
        [XmlEnum("point_hoist")] PointHoist,
        [XmlEnum("rotary")] Rotary,
        [XmlEnum("other")] Other
    }
    
    // Enum for positioning capability
    public enum AxisPositioning
    {
        // ANSI E1.44-2014 R2024 requires a string enum rather than boolean for positioning
        [XmlEnum("yes")] Yes,
        [XmlEnum("no")] No
    }

    // Enum for speed type
    public enum AxisSpeedType
    {
        // Constant Velocity
        [XmlEnum("fixed")] Fixed,
        // Velocity can be controlled by control system
        [XmlEnum("variable")] Variable
    }

    // Basic Axis model made in accordance with ANSI E1.44-2014 R2024
    [XmlType("b_axis")]
    public class Axis
    {
        // Parameterless constructor for xml serialization and deserialization
        public Axis() { }

        // ------------------ESTA PROPERTIES------------------------------
        // Only the id is required (according to E1.44-2014 R2024)
        [XmlAttribute("b_id")]
        public required int Id { get; set; }

        // Everything else is optional
        [XmlElement("b_name")]
        public string? Name { get; set; }

        [XmlElement("b_notes")]
        public string? Notes { get; set; }

        [XmlElement("b_interactive_decision_point")]
        public string? InteractiveDecisionPoint { get; set; }

        [XmlElement("b_type")]
        public AxisType? Type { get; set; }

        [XmlElement("b_location")]
        public AxisLocation? Location { get; set; }

        // Length of batten or diameter of revolve (mm). 0 or undefined for other machines
        [XmlElement("b_length")]
        public int? Length { get; set; }

        // Does the axis have an encoder/ is it capable of positioning
        [XmlElement("b_positioning")]
        public AxisPositioning? Positioning { get; set; }

        // Low limit position (mm or degrees)
        [XmlElement("b_low_limit")]
        public float? LowLimit { get; set; }

        // High limit position (mm or degrees)
        [XmlElement("b_high_limit")]
        public float? HighLimit { get; set; }

        [XmlElement("b_speed_type")]
        public AxisSpeedType? SpeedType { get; set; }

        // max speed in mm/s or degrees/s
        [XmlElement("b_max_speed")]
        public float? MaxSpeed { get; set; }

        // max accel in mm/s^2 or degrees/s^2
        [XmlElement("b_max_accel")]
        public float? MaxAccel { get; set; }

        // max decel in mm/s^2 or degrees/s^2
        [XmlElement("b_max_decel")]
        public float? MaxDecel { get; set; }

        // max load in kg
        [XmlElement("b_max_load")]
        public int? MaxLoad { get; set; }


        // ------------------AXISLINK SPECIFIC PROPERTIES------------------------------
        [XmlElement("alink_controller_id")]
        public int? ControllerId { get; set; }

        [XmlElement("alink_steps_per_rev")]
        public int? StepsPerRevolution { get; set; }

        // Distance the output moves per revolution of motor (circumference of output device typically), in mm or degrees
        // In the future, can auto calc this for some configurations
        [XmlElement("alink_dist_per_rev")]
        public float? DistancePerRevolution { get; set; }

        [XmlArray("alink_sensors")]
        [XmlArrayItem("alink_sensor")]
        public List<Sensor>? Sensors { get; set; } = [];

        // Type of interface. Currently only modbus is supported
        [XmlElement("alink_hardware_type")]
        public HardwareType? HardwareType { get; set; }

        // Config is polymorphic and can function for different motor/connection types
        [XmlElement("alink_hardware_config")]
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
