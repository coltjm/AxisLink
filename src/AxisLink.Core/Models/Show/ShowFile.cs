using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Show
{

    public class PatchList
    {
        
        [XmlElement("b_interactive_decision_point")]
        public string? InteractiveDecisionPoint { get; set; }
        [XmlElement("b_object")]
        public List<Patch>? Patches { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public PatchList() { }

        public PatchList(List<Patch> Patches) 
        {
            this.Patches = Patches;
        }
    }
    
    public class Machinery
    {
        
        [XmlArray("b_axes")]
        public List<Axis>? Axes { get; set; }
        [XmlArray("b_groups")]
        public List<Group>? Groups { get; set; }
        [XmlArray("b_scenery")]
        [XmlArrayItem("b_object")]
        public List<Scenery>? Scenery { get; set; }
        [XmlElement("b_patch")]
        public PatchList? Patches { get; set; }
        [XmlArray("b_cues")]
        public List<Cue>? Cues { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public Machinery() { }

        public Machinery(List<Axis> Axes, List<Group> Groups, List<Scenery> Scenery, List<Patch> Patches, List<Cue> Cues)
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
    public class ShowFile
    {
        [XmlElement("header")]
        public Header? Header { get; set; }

        [XmlElement("b_machinery")]
        public Machinery? Machinery { get; set; }

        [XmlArray("alink_controllers")]
        [XmlArrayItem("alink_controller")]
        public List<Controller>? Controllers { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public ShowFile() { }

        public ShowFile(List<Controller> Controllers, List<Axis> Axes, List<Group> Groups, List<Scenery> Scenery, List<Patch> Patches, List<Cue> Cues)
        {
            this.Machinery = new Machinery(Axes, Groups, Scenery, Patches, Cues);
            this.Header = new Header();
            this.Controllers = Controllers;
        }

    }
}
