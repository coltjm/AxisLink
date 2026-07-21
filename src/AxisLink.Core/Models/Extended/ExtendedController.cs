using AxisLink.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Extended
{
    public class ExtendedController
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string? Name { get; set; }

        [XmlElement("connection_config")]
        public ConnectionConfig? Config { get; set; }

        public ExtendedController() { }

        public ExtendedController(int id, string? name, ConnectionConfig? config)
        {
            Id = id;
            Name = name;
            Config = config;
        }
    }
}