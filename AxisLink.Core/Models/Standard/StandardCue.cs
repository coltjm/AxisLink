using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Standard
{
    
    [XmlType("b_cue")]
    [XmlInclude(typeof(ExtendedCue))]
    public class StandardCue
    {
        // Parameterless constructor for xml serialization and deserialization
        public StandardCue() { }
        // Cue number should be in the for "major.minor" with major not exceeding 3 digits (1-999) and minor not exceeding 2 digits (0-99)
        [XmlElement("b_number")]
        public required string Number { get; set; }

        [XmlElement("b_name")]
        public string? Name { get; set; }

        [XmlElement("b_notes")]
        public string? Notes { get; set; }

        [XmlElement("b_interactive_decision_point")]
        public string? InteractiveDecisionPoint { get; set; }

        // Which cue stack does this cue belong to
        [XmlElement("b_stack")]
        public int? Stack {  get; set; }

        [XmlElement("b_object")]
        public List<StandardCuePart> CueParts { get; set; } = new();

    }
}
