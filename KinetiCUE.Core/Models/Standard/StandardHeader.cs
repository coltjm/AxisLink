using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.Standard
{
    public class Date
    {
        [XmlElement("year")]
        public required int Year { get; set; }

        [XmlElement("month")]
        public required int Month { get; set; }

        [XmlElement("day")]
        public required int Day { get; set; }

        [XmlElement("hour")]
        public required int Hour { get; set; }

        [XmlElement("minute")]
        public required int Minute { get; set; }

        [XmlElement("second")]
        public required int Second { get; set; }
    }

    public class StandardHeader
    {
        [XmlElement("show_name")]
        public string? ShowName { get; set; }

        [XmlElement("notes")]
        public string? Notes { get; set; }

        [XmlElement("user")]
        public string? User {  get; set; }

        [XmlElement("date")]
        public Date? Date { get; set; }

        [XmlArray("versions")]
        [XmlArrayItem("version")]
        public List<string>? Version { get; set; }

    }
}
