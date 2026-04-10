using KinetiCUE.Core.Models.KQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Standard
{

    public class PatchList
    {
        
        [XmlElement("b_interactive_decision_point")]
        public string? InteractiveDecisionPoint { get; set; }
        [XmlElement("b_object")]
        public List<StandardPatch>? Patches { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public PatchList() { }

        public PatchList(List<StandardPatch> Patches) 
        {
            this.Patches = Patches;
        }
    }
    
    public class Machinery
    {
        
        [XmlArray("b_axes")]
        public List<StandardAxis>? Axes { get; set; }
        [XmlArray("b_groups")]
        public List<StandardGroup>? Groups { get; set; }
        [XmlArray("b_scenery")]
        [XmlArrayItem("b_object")]
        public List<StandardScenery>? Scenery { get; set; }
        [XmlElement("b_patch")]
        public PatchList? Patches { get; set; }
        [XmlArray("b_cues")]
        public List<StandardCue>? Cues { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public Machinery() { }

        public Machinery(List<StandardAxis> Axes, List<StandardGroup> Groups, List<StandardScenery> Scenery, List<StandardPatch> Patches, List<StandardCue> Cues)
        { 
            this.Axes = Axes;
            this.Groups = Groups;
            this.Scenery = Scenery;
            this.Patches = new PatchList(Patches);
            this.Cues = Cues;
        }


    }

    [XmlRoot("showfile")]
    [XmlType("standard_file")]
    public class StandardFile
    {
        [XmlElement("header")]
        public StandardHeader? Header { get; set; }

        [XmlElement("b_machinery")]
        public Machinery? Machinery { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public StandardFile() { }

        public StandardFile(List<StandardAxis> Axes, List<StandardGroup> Groups, List<StandardScenery> Scenery, List<StandardPatch> Patches, List<StandardCue> Cues)
        {
            this.Machinery = new Machinery(Axes, Groups, Scenery, Patches, Cues);
            this.Header = new StandardHeader();
        }
    }
}
