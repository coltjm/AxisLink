using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Configs
{
    public enum LinearUnitEnum
    {
        [XmlEnum("mm")] Millimeter,
        [XmlEnum("cm")] Centimeter,
        [XmlEnum("m")] Meter,
        [XmlEnum("in")] Inch,
        [XmlEnum("ft")] Foot,
        [XmlEnum("custom")] Custom // If custom, user must define number of mm per unit
    }

    public enum RotationalUnitEnum
    {
        [XmlEnum("deg")] Degree,
        [XmlEnum("rad")] Radian,
        [XmlEnum("rev")] Revolution,
        [XmlEnum("custom")] Custom  // If custom, user must define number of degrees per unit
    }

    public class ProjectConfig
    {
        // Default jump between consecutive cues
        [XmlElement("alink_cue_spacing")]
        public float CueSpacing { get; set; } = 5.0f;

        [XmlElement("alink_linear_unit")]
        public LinearUnitEnum LinearUnit { get; set; } = LinearUnitEnum.Millimeter;
        // Allow for non standard units if they make more sense for a certain show
        [XmlElement("alink_custom_linear_unit")]
        public float? CustomLinearUnit;

        [XmlElement("alink_rotational_unit")]
        public RotationalUnitEnum RotationalUnit { get; set; } = RotationalUnitEnum.Degree;
        // Allow for non standard units if they make more sense for a certain show i.e. hours for a clock
        [XmlElement("alink_custom_rotational_unit")] 
        public float? CustomRotationationalUnit;

        // Enables multiple cue stacks for multiple go's
        [XmlElement("alink_enable_stacks")]
        public bool EnableStacks = true;

        [XmlElement("alink_logger_display_cap")]
        public int LoggerDisplayCap = 1000;
    }
}
