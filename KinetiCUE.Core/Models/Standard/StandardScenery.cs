using KinetiCUE.Core.Models.KQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Standard
{

    public class SceneryTrims
    {
        // Lower limit of element
        [XmlElement("b_lowtrim")]
        public SceneryLimitTrim? LowTrim { get; set; }

        // Upper limit of element
        [XmlElement("b_hightrim")]
        public SceneryLimitTrim? HighTrim { get; set; }

        // Saved trim positions
        [XmlElement("b_trim")]
        public List<SceneryTrim>? Trims { get; set; } = new();

        // Parameterless constructor for xml serialization and deserialization
        public SceneryTrims() { }
    }

    public class SceneryLimitTrim
    {
        [XmlElement("b_name")]
        public string? Name { get; set; }

        [XmlElement("b_notes")]
        public string? Notes { get; set; }

        // Position in mm according to E1.44-2014 R2024 5.3.1.2
        [XmlElement("b_position")]
        public float? Position { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public SceneryLimitTrim() { }
    }

    public class SceneryTrim
    {
        [XmlAttribute("b_id")]
        public required int Id { get; set; }

        [XmlElement("b_name")]
        public string? Name { get; set; }

        [XmlElement("b_notes")]
        public string? Notes { get; set; }

        // Position in mm according to E1.44-2014 R2024 5.3.1.2
        [XmlElement("b_position")]
        public float? Position { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public SceneryTrim() { }
    }

    [XmlType("b_scenery_object")]
    [XmlInclude(typeof(KQScenery))]
    public class StandardScenery
    {
        [XmlAttribute("b_id")]
        public required int Id { get; set; }

        [XmlElement("b_name")]
        public string? Name { get; set; }

        [XmlElement("b_notes")]
        public string? Notes { get; set; }

        [XmlElement("b_interactive_decision_point")]
        public string? InteractiveDecisionPoint { get; set; }

        // Height of object incliding attachment hardware in mm 
        [XmlElement("b_height")]
        public int? Height { get; set; }

        //  Weight of object in kg
        [XmlElement("b_weight")]
        public int? Weight { get; set; }

        // max speed in mm/s or degrees/s
        [XmlElement("b_speed_limit")]
        public float? SpeedLimit { get; set; }

        [XmlElement("b_trims")]
        public SceneryTrims? Trims { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public StandardScenery() { }


    }
}
