using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Standard
{
    // Must be wrapped in "b_patch" with a "b_interactive_decision_point" for entire list (if decision point is needed)

    [XmlType("b_object")]
    public class StandardPatch
    {
        // Id corresponding to a scenery object
        [XmlAttribute("b_id")]
        public required int Id { get; set; }

        // Each scenery object corresponds to either an axis or a group

        [XmlElement("b_axis")]
        public int? AxisId { get; set; }

        [XmlElement("b_group")]
        public int? GroupId { get; set; }
    }
}
