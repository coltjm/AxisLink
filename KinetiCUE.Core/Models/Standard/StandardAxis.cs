using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Standard
{
    // Location subclass specific to Axes in E1.44-2014 R2024
    public class AxisLocation
    {
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
    public class StandardAxis
    {
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
    }
}
