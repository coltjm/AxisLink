using KinetiCUE.Core.Models.Standard;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.KQ
{
    [XmlRoot("showfile")]
    [XmlType("kq_file")]
    public class KQFile : StandardFile
    {
        [XmlArray("controllers")]
        [XmlArrayItem("controller")]
        public List<KQController> Controllers { get; set; }
        public KQFile(List<KQController> controllers, List<StandardAxis> Axes, List<StandardGroup> Groups, List<StandardScenery> Scenery, List<StandardPatch> Patches, List<StandardCue> Cues) 
            : base(Axes, Groups, Scenery, Patches,  Cues)
        {
            this.Controllers = controllers;
        }

        // Parameterless constructor for xml serialization and deserialization
        public KQFile() { }
    }
}
