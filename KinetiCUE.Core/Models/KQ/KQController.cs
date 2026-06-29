using KinetiCUE.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.KQ
{
    public class KQController
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string? Name { get; set; }

        [XmlElement("connection_config")]
        public ConnectionConfig? Config { get; set; }

        public KQController() { }
    }
}