using AxisLink.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Extended
{
    public class ExtendedPLC
    {
        // Parameterless constructor for xml serialization and deserialization
        public ExtendedPLC() { }
        [XmlAttribute("id")]
        public required int Id { get; set; }

        [XmlElement("name")]
        public string? Name { get; set; }
        
        [XmlElement("connection_config")]
        public ConnectionConfig? ConnectionConfig { get; set; }
    }
}
