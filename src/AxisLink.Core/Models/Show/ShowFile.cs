using AxisLink.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using AxisLink.Core.Models.Sprockets;

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
        public PatchList? PatchList { get; set; }
        [XmlArray("b_cues")]
        public List<Cue>? Cues { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public Machinery() { }

        public Machinery(List<Axis> Axes, List<Group> Groups, List<Scenery> Scenery, List<Patch> PatchList, List<Cue> Cues)
        { 
            this.Axes = Axes;
            this.Groups = Groups;
            this.Scenery = Scenery;
            this.PatchList = new PatchList(PatchList);
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

        [XmlArray("alink_sensors")]
        [XmlArrayItem("alink_sensor")]
        public List<Sensor>? Sensors { get; set; } = [];

        [XmlElement("alink_project_config")]
        public ProjectConfig? ProjectConfig { get; set; }

        [XmlArray("alink_embedded_sprockets")]
        [XmlArrayItem("alink_sprocket_payload")]
        public List<string>? EmbeddedSprockets { get; set; }

        // Parameterless constructor for xml serialization and deserialization
        public ShowFile() { }

        public ShowFile(List<Controller> Controllers, List<Sensor> Sensors, List<Axis> Axes, List<Group> Groups, 
            List<Scenery> Scenery, List<Patch> Patches, List<Cue> Cues, ProjectConfig? projectConfig, List<string>? embeddedSprockets = null)
        {
            this.Machinery = new Machinery(Axes, Groups, Scenery, Patches, Cues);
            this.Header = new Header();
            this.Controllers = Controllers;
            this.Sensors = Sensors;
            this.EmbeddedSprockets = embeddedSprockets;
            this.ProjectConfig = projectConfig!=null? projectConfig: new();
        }

    }
}
