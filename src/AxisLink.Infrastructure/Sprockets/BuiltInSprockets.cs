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

            Registers = new Dictionary<string, string>
            {
                ["target_pos"] = "16388 + ($channel - 1) * 20",
                ["target_vel"] = "16390 + ($channel - 1) * 20",
                ["target_accel"] = "16392 + ($channel - 1) * 20",
                ["target_decel"] = "16394 + ($channel - 1) * 20",
                ["data_available"] = "100035 + ($channel - 1)",
                ["go_cue"] = "100033 + ($channel - 1)",
                ["go_jog"] = "100034 + ($channel - 1)",
                ["busy"] = "16385 + ($channel - 1)",
                ["error_code"] = "1 + ($channel - 1)"
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
                new() { Target = "go_cue", Value = "0" }
            }
        };

    }
}
