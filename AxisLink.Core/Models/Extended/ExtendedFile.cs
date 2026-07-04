using AxisLink.Core.Models.Standard;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Extended
{
    [XmlRoot("showfile")]
    [XmlType("alink_file")]
    public class ExtendedFile : StandardFile
    {
        [XmlArray("controllers")]
        [XmlArrayItem("controller")]
        public List<ExtendedController> Controllers { get; set; }
        public ExtendedFile(List<ExtendedController> controllers, List<StandardAxis> Axes, List<StandardGroup> Groups, List<StandardScenery> Scenery, List<StandardPatch> Patches, List<StandardCue> Cues) 
            : base(Axes, Groups, Scenery, Patches,  Cues)
        {
            this.Controllers = controllers;
        }

        // Parameterless constructor for xml serialization and deserialization
        public ExtendedFile() { }
    }
}
