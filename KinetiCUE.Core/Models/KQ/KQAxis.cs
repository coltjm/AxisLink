using KinetiCUE.Core.Models.Standard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace KinetiCUE.Core.Models.KQ
{
    [XmlType("kq_axis")]
    public class KQAxis : StandardAxis
    {
        [XmlElement("plc")]

    }
}
