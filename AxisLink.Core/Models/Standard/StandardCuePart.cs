using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Standard
{
    // Defines the types of moves according to E1.44-2014 R2024
    public enum CuePartMoveType
    {
        // a direct move to the target with programmed accel-, velocity and decel-ramps
        [XmlEnum("linear")] Linear,
        // for rotary axes a: direct counter clockwise move with programmed accel,- velocity and decal-ramps
        [XmlEnum("rotary_ccw")] RotaryCCW,
        // for rotary axes a: direct clockwise move with programmed accel,- velocity and decal-ramps
        [XmlEnum("rotary_cw")] RotaryCW,
        // for rotary axes a: direct move with the shortest angle with programmed accel,- velocity and decal-ramps
        [XmlEnum("rotary_shortest")] RotaryShortest,
        // for axes without end-stops (e.g. rotary, conveyer belts): the start of a continuous move in the direction of larger position
        // or degree numbers with programmed accel-ramp and velocity
        [XmlEnum("continuous_increasing")] ContinuousIncreasing,
        // for axes without end-stops (e.g. rotary, conveyer belts): the start of a continuous move in the direction of smaller position
        // or degree numbers with programmed accel-ramp and velocity
        [XmlEnum("continuous_decreasing")] ContinuousDecreasing,
        // the scenery object is connected to manual joystick control in this cue.The joystick utilized shall be identified by the “playback” entry
        [XmlEnum("joystick")] Joystick
    }

    // What type of position is used as the start position
    public enum CuePartStartType
    {
        // Reference to limit stored in scenery object
        [XmlEnum("limit")] Limit,
        // Reference to trim stored in scenery object
        [XmlEnum("trim")] Trim,
        // Absolute position
        [XmlEnum("absolute")] Absolute
    }

    // What type of position is used as the target position?
    public enum CuePartTargetType
    {
        // Reference to limit stored in scenery object
        [XmlEnum("limit")] Limit,
        // Reference to trim stored in scenery object
        [XmlEnum("trim")] Trim,
        // Absolute position
        [XmlEnum("absolute")] Absolute,
        // Distance from current position
        [XmlEnum("relative")] Relative
    }

    [XmlType("b_object")]
    [XmlInclude(typeof(ExtendedCuePart))]
    public class StandardCuePart
    {
        // Parameterless constructor for xml serialization and deserialization
        public StandardCuePart() { }
        // Scenery reference ID
        [XmlAttribute("b_id")]
        public required int Id { get; set; }

        // Which user/console plays this part of the cue
        [XmlElement("b_user")]
        public string? User { get; set; }

        // information on which playback this part of the cue is loaded
        [XmlElement("b_playback")]
        public int? Playback { get; set; }

        [XmlElement("b_move_type")]
        public CuePartMoveType? MoveType { get; set; }

        [XmlElement("b_start")]
        public CuePartStart? Start { get; set; }

        // Required if move type is linear or multitarget
        [XmlElement("b_target")]
        public CuePartTarget? Target { get; set; }
    }

    public class CuePartStart
    {
        [XmlElement("b_type")]
        public CuePartStartType? Type { get; set; }

        // Name of limit within scenery object if start type is limit (I assume this references the lowtrim and hightrim values although documentation is slightly vague imo)
        [XmlElement("b_limit")]
        public string? Limit { get; set; }

        // ID of trim position within scenery object if start type is trim
        [XmlElement("b_trim")]
        public int? Trim { get; set; }

        // Absolute position of the position the object should start in if start type is absolute (Position in mm according to E1.44-2014 R2024 5.3.1.2)
        [XmlElement("b_position")]
        public int? Position { get; set; }
    }

    public class CuePartTarget
    {
        [XmlElement("b_type")]
        public CuePartTargetType? Type { get; set; }

        // Name of limit within scenery object if target type is limit (I assume this references the lowtrim and hightrim values although documentation is slightly vague imo)
        [XmlElement("b_limit")]
        public string? Limit { get; set; }

        // ID of trim position within scenery object if target type is trim
        [XmlElement("b_trim")]
        public int? Trim { get; set; }

        // Absolute position of the position the object should end at if target type is absolute (Position in mm according to E1.44-2014 R2024 5.3.1.2)
        [XmlElement("b_position")]
        public int? Position { get; set; }

        // Absolute distance in mm or degrees if target type is relative. to be added to current position to get target position
        [XmlElement("b_distance")]
        public float? Distance { get; set; }

        // time between go press and motion start in seconds
        [XmlElement("b_delay")]
        public float? Delay { get; set; }

        // Total time cue should take from pressing go in seconds. Purely information
        [XmlElement("b_time")]
        public float? Time { get; set; }

        // accel in mm/s^2 or degrees/s^2
        [XmlElement("b_accel")]
        public float? Accel { get; set; }

        // decel in mm/s^2 or degrees/s^2
        [XmlElement("b_decel")]
        public float? Decel { get; set; }

        // Max/target speed of move in mm/s or degrees/s. Required
        [XmlElement("b_speed")]
        public required float Speed { get; set; }
    }

}
