using System;
using System.Collections.Generic;
using System.Text;
using AxisLink.Core.Models.Sprockets;
using AxisLink.Infrastructure.Mappers;

namespace AxisLink.Infrastructure.Sprockets
{
    public class BuiltInSprockets
    {
        public static readonly Sprocket ClickPlcStepper = new()
        {
            Id = "click_plus_plc_stepper_v1",
            DisplayName = "AutomationDirect CLICK Plus - Modbus TCP Stepper (V1)",
            Protocol = TransportProtocol.ModbusTcp,
            DefaultDriveScaleFactor = 2000f,

            AddressAliases = new Dictionary<string, SprocketModbusTarget>
            {
                ["target_pos"] = new SprocketModbusTarget{Address = "16388 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16},
                ["target_vel"] = new SprocketModbusTarget { Address = "16390 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16},
                ["target_accel"] = new SprocketModbusTarget { Address = "16392 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16 },
                ["target_decel"] = new SprocketModbusTarget { Address = "16394 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16 },
                ["data_available"] = new SprocketModbusTarget { Address = "100035 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["go_cue"] = new SprocketModbusTarget { Address = "100033 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["go_jog"] = new SprocketModbusTarget { Address = "100034 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["busy"] = new SprocketModbusTarget { Address = "16385 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["error_code"] = new SprocketModbusTarget { Address = "1 + ($channel - 1)", ModbusDataType = ModbusDataType.Register16 }
            },

            RunCueSequence = new List<SprocketStep>
            {
                new() { Target = "target_pos", Value = "$position" },
                new() { Target = "target_vel", Value = "$velocity" },
                new() { Target = "target_accel", Value = "$accel" },
                new() { Target = "target_decel", Value = "$decel" },
                new() { Target = "data_available", PulseMs = 25 },
                new() { DelayMs = 10 },
                new() { Target = "go_cue", PulseMs = 50 }
            },

            RunJogSequence = new List<SprocketStep>
            {
                new() { Target = "target_vel", Value = "$velocity" },
                new() { Target = "target_accel", Value = "$accel" },
                new() { Target = "target_decel", Value = "$decel" },
                new() { Target = "go_jog", Value = "1" }
            },

            StopSequence = new List<SprocketStep>
            {
                new() { Target = "go_jog", Value = "0" },
                new() { Target = "go_cue", Value = "0" }
            }
        };

        public static readonly Sprocket SimulatedAxis = new()
        {
            Id = "simulated_axis_v1",
            DisplayName = "AxisLink Virtual / Simulated Axis (Dev Port)",
            Protocol = TransportProtocol.ModbusTcp,
            DefaultDriveScaleFactor = 2000f,

            AddressAliases = new Dictionary<string, SprocketModbusTarget>
            {
                ["target_pos"] = new SprocketModbusTarget { Address = "16388 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16 },
                ["target_vel"] = new SprocketModbusTarget { Address = "16390 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16 },
                ["target_accel"] = new SprocketModbusTarget { Address = "16392 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16 },
                ["target_decel"] = new SprocketModbusTarget { Address = "16394 + ($channel - 1) * 20", ModbusDataType = ModbusDataType.Register16 },
                ["data_available"] = new SprocketModbusTarget { Address = "100035 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["go_cue"] = new SprocketModbusTarget { Address = "100033 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["go_jog"] = new SprocketModbusTarget { Address = "100034 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["busy"] = new SprocketModbusTarget { Address = "16385 + ($channel - 1)", ModbusDataType = ModbusDataType.Coil },
                ["error_code"] = new SprocketModbusTarget { Address = "1 + ($channel - 1)", ModbusDataType = ModbusDataType.Register16 }
            },

            RunCueSequence = new List<SprocketStep>
            {
                new() { Target = "target_pos", Value = "{Target.Position}" },
                new() { Target = "target_vel", Value = "{Target.Velocity}" },
                new() { Target = "target_accel", Value = "{Target.Accel}" },
                new() { Target = "target_decel", Value = "{Target.Decel}" },
                new() { Target = "data_available", PulseMs = 25 },
                new() { DelayMs = 10 },
                new() { Target = "go_cue", PulseMs = 50 }
            },

                    RunJogSequence = new List<SprocketStep>
            {
                new() { Target = "target_vel", Value = "{Target.Velocity}" },
                new() { Target = "target_accel", Value = "{Target.Accel}" },
                new() { Target = "target_decel", Value = "{Target.Decel}" },
                new() { Target = "go_jog", Value = "1" }
            },

                    StopSequence = new List<SprocketStep>
            {
                new() { Target = "go_jog", Value = "0" },
                new() { Target = "go_cue", Value = "0" },
                new() { Target = "stop", PulseMs = 50 }
            }
                };

            }
}
