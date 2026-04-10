using KinetiCUE.Core.Models.KQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Standard
{
    // Enum for group type
    public enum GroupType
    {
        // independent axes, a fault on an axis does not stop other axes of the group
        [XmlEnum("free")] Free,
        // a fault on any axis of the group stops all axes of the group
        [XmlEnum("safe")] Safe,
        // full position synchronization of all axis in the group, a fault on any axis of the group stops all axes of the group
        [XmlEnum("locked")] Locked
    }

    // This axis is taken as the position reference of the group both for display and targeting purposes.
    public class GroupMasterAxis
    {
        // Parameterless constructor for xml serialization and deserialization
        public GroupMasterAxis() { }
        [XmlAttribute("b_id")]
        public required int Id { get; set; }
    }

    // Axis subclass in E1.44-2014 R2024
    public class GroupAxis
    {
        // Parameterless constructor for xml serialization and deserialization
        public GroupAxis() { }
        [XmlAttribute("b_id")]
        public required int Id { get; set; }

        // Relative position offset compared to master in mm or degrees
        [XmlElement("b_offset")]
        public float? Offset { get; set; }

    }

    // Basic Axis model made in accordance with ANSI E1.44-2014 R2024
    [XmlType("b_group")]
    [XmlInclude(typeof(KQGroup))]
    public class StandardGroup
    {
        [XmlAttribute("b_id")]
        public required int Id { get; set; }

        [XmlElement("b_name")]
        public string? Name { get; set; }

        [XmlElement("b_notes")]
        public string? Notes { get; set; }

        [XmlElement("b_interactive_decision_point")]
        public string? InteractiveDecisionPoint { get; set; }

        [XmlElement("b_type")]
        public required GroupType Type { get; set; }

        // max speed in mm/s or degrees/s
        [XmlElement("b_speed_limit")]
        public float? SpeedLimit { get; set; }

        [XmlElement("b_master_axis")]
        public required GroupMasterAxis MasterAxis { get; set; }

        [XmlElement("b_axis")]
        public List<GroupAxis> GroupAxes { get; set; } = new();

        // Parameterless constructor for xml serialization and deserialization
        public StandardGroup() { }


    }
}
