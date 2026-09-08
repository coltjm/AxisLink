using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Sprockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Show
{
    public class Controller
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string? Name { get; set; }

        [XmlElement("ip_address")]
        public string IpAddress { get; set; } = "127.0.0.1";

        [XmlElement("port")]
        public int Port { get; set; } = 502;

        [XmlElement("protocol")]
        public TransportProtocol Protocol { get; set; } = TransportProtocol.ModbusTcp;

        // Optional timeout in milliseconds
        [XmlElement("timeout_ms")]
        public int TimeoutMs { get; set; } = 1000;

        public Controller() { }

        public Controller(int id, string? name, string ipAddress, int port, TransportProtocol protocol)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            Port = port;
            Protocol = protocol;
        }
    }
}