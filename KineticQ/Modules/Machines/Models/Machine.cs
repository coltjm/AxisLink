using CommunityToolkit.Mvvm.ComponentModel;

using System.Text.Json.Serialization;
using KineticQ.Modules.Cueing.Models;

namespace KineticQ.Modules.Machines.Models
{
    public partial class Machine : ObservableObject
    {
        // --- CONFIGURATION (Saved) ---
        public int Id { get; set; }

        public string Name { get; set; } = "New Axis";
        public float MaxVelocity { get; set; } = 24.0f;
        public float MaxPosition { get; set; } = 1200.0f;
        public float MinPosition { get; set; } = 0.0f;
        public float Home { get; set; } = 0.0f;
        public float StepsPerRevolution { get; set; } = 1600.0f;
        public float DistPerRevolution { get; set; } = 4.5f;
        public bool IsLinear { get; set; } = true; // Linear vs Rotary

        // --- NETWORK ---
        public string IPAddress { get; set; } = "192.168.0.10";
        public int Port { get; set; } = 502;
        public int UnitId { get; set; } = 1;

        // --- MODBUS MAPPING (The Form Data) ---
        // Note: We store these as STRINGS because users might type "40001" or "40001.1"
        // or you might want to support "C1" style naming later.

        // Commands (Write)
        public string Addr_MoveCmd { get; set; } = "16385";      // Coil
        public string Addr_TargetPos { get; set; } = "428673";   // Float
        public string Addr_TargetVel { get; set; } = "428675";   // Float

        // Feedback (Read)
        public string Addr_CurrentPos { get; set; } = "428677";  // Float
        public string Addr_StatusBit { get; set; } = "16484";     // Discrete Input
        public string Addr_FaultBit { get; set; } = "16485";    // Discrete Input


        // --- LIVE TELEMETRY (Not Saved) ---

        // Motion
        [JsonIgnore][ObservableProperty] private float _currentPosition;
        [JsonIgnore][ObservableProperty] private float _targetPosition;
        [JsonIgnore][ObservableProperty] private float _currentVelocity;
        [JsonIgnore][ObservableProperty] private float _targetVelocity;
        [JsonIgnore][ObservableProperty] private float _currentLoad; // % of Torque

        // Status Flags (The "Idiot Lights")
        [JsonIgnore][ObservableProperty] private bool _isConnected;
        [JsonIgnore][ObservableProperty] private bool _isEnabled;     // Motor On/Off
        [JsonIgnore][ObservableProperty] private bool _isBrakeReleased;
        [JsonIgnore][ObservableProperty] private bool _isFwdLimit;    // Hit forward wall?
        [JsonIgnore][ObservableProperty] private bool _isRevLimit;    // Hit reverse wall?

        // Faults
        [JsonIgnore][ObservableProperty] private bool _isFaulted;
        [JsonIgnore][ObservableProperty] private string _faultMessage = "NO FAULT";

        // --- CONSTRUCTOR ---
        public Machine(
            int id, string name, float maxVelocity,
            float maxPosition, float minPosition, float homePosition,
            float stepsPerRev, float distPerRev, bool isLinear, string IPAddress,
            int port, int unitId, string Addr_MoveCmd, string Addr_TargetPos,
            string Addr_TargetVel, string Addr_CurrentPos, string Addr_StatusBit, string Addr_FaultBit)
        {
            Id = id;
            Name = name;
            MaxPosition = maxPosition;
            MinPosition = minPosition;
            Home = homePosition;
            StepsPerRevolution = stepsPerRev;
            DistPerRevolution = distPerRev;
            IsLinear = isLinear;
            this.IPAddress = IPAddress;
            Port = port;
            UnitId = unitId;
            this.Addr_MoveCmd = Addr_MoveCmd;
            this.Addr_TargetPos = Addr_TargetPos;
            this.Addr_TargetVel = Addr_TargetVel;
            this.Addr_CurrentPos = Addr_CurrentPos;
            this.Addr_StatusBit = Addr_StatusBit;
            this.Addr_FaultBit = Addr_FaultBit;
            // Initialize defaults
            CurrentPosition = homePosition;
            TargetPosition = homePosition;
        }

        public Machine() { }

        // --- LOGIC ---
        public bool IsReady()
        {
            // Example logic: Ready if not faulted and E-Stop is clear
            return !IsFaulted;
        }

        public bool RunCue(Cue cue)
        {
            //if (!IsReady() || cue.MachineId != Id) return false;

            //// In a real app, this would send a packet to the PlcService
            //ExpectedPosition = cue.TargetPosition;
            //ExpectedVelocity = cue.TargetVelocity;
            //IsRunning = true;
            //IsBraked = false;

            return true;
        }
    }
}