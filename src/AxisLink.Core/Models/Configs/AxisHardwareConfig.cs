using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AxisLink.Core.Models.Configs
{
    [XmlInclude(typeof(ModbusStepperConfig))]
    public abstract class AxisHardwareConfig
    {
        // Parameterless constructor for xml serialization and deserialization
        public AxisHardwareConfig() { }


        // Common properties for all axis hardware
    }

    public class ModbusStepperConfig : AxisHardwareConfig
    {
        // Parameterless constructor for xml serialization and deserialization
        public ModbusStepperConfig() { }
        // Motor Driver addresses
        // Y### - OUTPUT COIL
        [XmlElement("pulse_addr")]
        public string? PulseAddress { get; set; }
        // Y### - OUTPUT COIL
        [XmlElement("direction_addr")]
        public string? DirectionAddress { get; set; }
        // Y### - OUTPUT COIL
        [XmlElement("enable_addr")]
        public string? EnableAddress { get; set; }
        // Y### - OUTPUT COIL
        [XmlElement("brake_addr")]
        public string? BrakeAddress { get; set; }
        // X### - INPUT COIL
        // READ IN POLL
        [XmlElement("alarm_addr")]
        public string? AlarmAddress { get; set; }
        

        // PLC Communication addresses (send information to and from plc, do not directly cause movement)

        // All necessary info has been sent (destination, velocity, etc)
        [XmlElement("data_available_addr")]
        public string? DataAvailable {  get; set; }
        // PLC has recieved info - likely redundant and not needed
        [XmlElement("destination_loaded_addr")]
        public string? DestinationLoaded { get; set; }
        // Whether or not this axis is in motion
        // READ IN POLL
        [XmlElement("busy_addr")]
        public string? Busy {  get; set; }
        // Go signal for this axis
        [XmlElement("go_addr")]
        public string? Go {  get; set; }
        // If we are in Safe Torque Off
        [XmlElement("safe_torque_off_addr")]
        public string? STO { get; set; }
        // If we are in Safe Stop 1
        [XmlElement("safe_stop_1_addr")]
        public string? SS1 { get; set; }
        // If we are in Safe Stop 2
        [XmlElement("safe_stop_2_addr")]
        public string? SS2 { get; set; }
        // Go signal for jog on this axis
        [XmlElement("jog_go_addr")]
        public string? JogGo { get; set; }
        // Whether or not PLC encountered error during move
        // READ IN POLL
        [XmlElement("error_addr")]
        public string? Error { get; set; }
        // Error code of error if applicable
        // READ IN POLL
        [XmlElement("error_code_addr")]
        public string? ErrorCode { get; set; }
        // If position move was completed (cue motion)
        // READ IN POLL IF MOVE EXPECTED
        [XmlElement("pm_complete_addr")]
        public string? PMComplete { get; set; }
        // If position move was successful (cue motion)
        // READ IN POLL IF MOVE EXPECTED
        [XmlElement("pm_success_addr")]
        public string? PMSuccess {  get; set; }
        // If velocity move was completed (jog motion)
        // READ IN POLL IF MOVE EXPECTED
        [XmlElement("vm_complete_addr")]
        public string? VMComplete { get; set; }
        // If velocity move was successful (jog motion)
        // READ IN POLL IF MOVE EXPECTED
        [XmlElement("vm_success_addr")]
        public string? VMSuccess { get; set; }
        [XmlElement("target_position_addr")]
        public string? TargetPosition { get; set; }
        [XmlElement("target_velocity_addr")]
        public string? TargetVelocity { get; set; }
        [XmlElement("target_accel_addr")]
        public string? TargetAccel { get; set; }
        [XmlElement("target_decel_addr")]
        public string? TargetDecel { get; set; }
        // Strength of S curve 0-100%
        [XmlElement("target_s_curve_addr")]
        public string? TargetSCurve { get; set; }
        [XmlElement("target_jog_velocity_addr")]
        public string? TargetJogVelocity { get; set; }
        [XmlElement("target_jog_accel_addr")]
        public string? TargetJogAccel { get; set; }
        [XmlElement("target_jog_decel_addr")]
        public string? TargetJogDecel { get; set; }
        // READ IN POLL
        [XmlElement("current_position_addr")]
        public string? CurrentPosition { get; set; }
        // READ IN POLL
        [XmlElement("current_velocity_addr")]
        public string? CurrentVelocity { get; set; }

    }

}
