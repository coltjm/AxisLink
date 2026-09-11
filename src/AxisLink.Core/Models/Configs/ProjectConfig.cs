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

    public class Shortcuts
    {
        public Shortcuts() { }

        // File
        [XmlElement("alink_save_shortcut")]
        public string SaveShortcut { get; set; } = "Ctrl+S";
        [XmlElement("alink_save_as_shortcut")]
        public string SaveAsShortcut { get; set; } = "Ctrl+Shift+S";
        [XmlElement("alink_open_shortcut")]
        public string OpenShortcut { get; set; } = "Ctrl+O";
        [XmlElement("alink_new_shortcut")]
        public string NewShortcut { get; set; } = "Ctrl+N";
        [XmlElement("alink_preferences_shortcut")]
        public string PreferencesShortcut { get; set; } = "Ctrl+,";

        // Windows
        [XmlElement("alink_cue_shortcut")]
        public string CueShortcut { get; set; } = "Ctrl+U";
        [XmlElement("alink_scenery_shortcut")]
        public string SceneryShortcut { get; set; } = "Ctrl+Y";
        [XmlElement("alink_axis_shortcut")]
        public string AxisShortcut { get; set; } = "Ctrl+A";
        [XmlElement("alink_controller_shortcut")]
        public string ControllerShortcut { get; set; } = "Ctrl+D";
        [XmlElement("alink_talk_shortcut")]
        public string TalkShortcut { get; set; } = "Ctrl+T";
        [XmlElement("alink_patch_shortcut")]
        public string PatchShortcut { get; set; } = "Ctrl+P";
        [XmlElement("alink_jog_shortcut")]
        public string JogShortcut { get; set; } = "Ctrl+J";
        [XmlElement("alink_group_shortcut")]
        public string GroupShortcut { get; set; } = "Ctrl+G";
        [XmlElement("alink_sensor_shortcut")]
        public string SensorShortcut { get; set; } = "Ctrl+I";

        // Modules
        [XmlElement("alink_logger_shortcut")]
        public string LoggerShortcut { get; set; } = "Ctrl+Shift+L";
        [XmlElement("alink_axis_viewer_shortcut")]
        public string AxisViewerShortcut { get; set; } = "Ctrl+Shift+A";
        [XmlElement("alink_controller_viewer_shortcut")]
        public string ControllerViewerShortcut { get; set; } = "Ctrl+Shift+D";
        [XmlElement("alink_cue_list_shortcut")]
        public string CueListViewerShortcut { get; set; } = "Ctrl+Shift+U";
        [XmlElement("alink_cue_run_shortcut")]
        public string CueRunShortcut { get; set; } = "Ctrl+Shift+R";
    }

    public class ProjectConfig
    {
        public ProjectConfig() { }

        [XmlElement("alink_theme")]
        public AppThemeConfig? Theme { get; set; } = AppThemeConfig.DefaultDark();

        // Default jump between consecutive cues
        [XmlElement("alink_cue_spacing")]
        public float CueSpacing { get; set; } = 5.0f;

        [XmlElement("alink_linear_unit")]
        public LinearUnitEnum LinearUnit { get; set; } = LinearUnitEnum.Millimeter;
        // Allow for non standard units if they make more sense for a certain show
        [XmlElement("alink_custom_linear_unit")]
        public float? CustomLinearUnit { get; set; }

        [XmlElement("alink_rotational_unit")]
        public RotationalUnitEnum RotationalUnit { get; set; } = RotationalUnitEnum.Degree;
        // Allow for non standard units if they make more sense for a certain show i.e. hours for a clock
        [XmlElement("alink_custom_rotational_unit")] 
        public float? CustomRotationationalUnit { get; set; }

        // Enables multiple cue stacks for multiple go's
        [XmlElement("alink_enable_stacks")]
        public bool EnableStacks { get; set; } = true;

        [XmlElement("alink_logger_display_cap")]
        public int LoggerDisplayCap { get; set; } = 1000;

        // Allow equations in numerical inputs
        [XmlElement("alink_math_in_input")]
        public bool MathInInput { get; set; } = true;

        [XmlElement("alink_shortcuts")]
        public Shortcuts? Shortcuts { get; set; } = new();
    }
}
