using KinetiCUE.Core.Models.Configs;
using KinetiCUE.Core.Models.KQ;
using KinetiCUE.Core.Models.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertions;

namespace KinetiCUE.Tests.Models.KQ
{
    public class KQSerializationTests
    {
        [Fact]
        public void KQFile_FullRoundTrip()
        {
            var controller0 = new KQController
            {
                Id = 0,
                Name = "Modbus Controller",
                Config = new ModbusConfig
                {
                    IpAddress = "192.168.1.100",
                    Port = "502",
                    EStopAddress = "X000"
                }

            };
            var controller1 = new KQController
            {
                Id = 1,
                Name = "Modbus Controller 2",
                Config = new ModbusConfig
                {
                    IpAddress = "192.168.1.101",
                    Port = "502",
                    EStopAddress = "X000"
                }

            };
            var sensor0 = new KQSensor
            {
                Id = 0,
                ControllerId = 0,
                SensorType = SensorTypes.Home,
                NC = true,
                Config = new ModbusSensorConfig
                {
                    SignalAddress = "X001"
                }
            };
            var axis0 = new KQAxis
            {
                Id = 0,
                ControllerId = 1,
                StepsPerRevolution = 200,
                DistancePerRevolution = 100,
                Sensors = new List<KQSensor>
                {
                    sensor0
                },
                HardwareType = HardwareType.Modbus,
                Config = new ModbusStepperConfig
                {
                    PulseAddress = "Y001",
                    DirectionAddress = "Y002"
                },
                IsEnabled = false,
                HasAlarm = false,
                CurrentPos = 100,
                CurrentVel = 0

            };
            var axis1 = new KQAxis
            {
                Id = 1,
                ControllerId = 1,
                StepsPerRevolution = 200,
                DistancePerRevolution = 100,
                Sensors = new(),
                HardwareType = HardwareType.Modbus,
                Config = new ModbusStepperConfig
                {
                    PulseAddress = "Y001",
                    DirectionAddress = "Y002"
                },
                IsEnabled = false,
                HasAlarm = false,
                CurrentPos = 300,
                CurrentVel = 0

            };
            var group0 = new KQGroup
            {
                Id = 0,
                Name = "main group",
                Type = GroupType.Free,
                SpeedLimit = 100,
                MasterAxis = new GroupMasterAxis { Id = 0 },
                GroupAxes = new List<GroupAxis>()
                {
                    new GroupAxis { Id = 1, Offset = 200}
                }
            };
            var scenery0 = new KQScenery 
            { 
                Id = 0,
                Name = "Tree Flyrail",
                Height = 400,
                Weight = 10,
                SpeedLimit = 100,
                Trims = new SceneryTrims 
                { 
                    LowTrim = new SceneryLimitTrim
                    {
                        Name="minimum",
                        Position = 100
                    },
                    HighTrim = new SceneryLimitTrim
                    {
                        Name="maximum",
                        Position = 1000
                    },
                    Trims = new List<SceneryTrim>()
                    {
                        new SceneryTrim
                        {
                            Id = 0,
                            Name = "home",
                            Position = 500
                        }
                    }
                }
            };
            var patch0 = new KQPatch
            { 
                // References scenery0
                Id = 0,
                // Connect scenery0 to group0
                GroupId = 0                
            };
            var cuePart0 = new KQCuePart 
            { 
                Id = 0,
                User = "FOH",
                Playback = 2,
                MoveType = CuePartMoveType.Linear,
                Start = new CuePartStart
                {
                    Type=CuePartStartType.Limit,
                    Limit = "maximum"
                },
                Target = new CuePartTarget
                {
                    Type = CuePartTargetType.Limit,
                    Limit = "minimum",
                    Delay = 1.5f,
                    Time = 10,
                    Accel = 30,
                    Decel = 30,
                    Speed = 50
                }
            };
            var cue0 = new KQCue
            { 
                Number = "1.23",
                Name = "Fly tree in",
                Stack = 2,
                CueParts = new List<StandardCuePart> { cuePart0 }
            };
            var header = new KQHeader 
            {
                ShowName = "KQTest",
                Notes = "Unit test for KQ development",
                User = "Colt McGuire",
                Date = new Date
                { 
                    Year = 2026,
                    Month = 4,
                    Day = 9,
                    Hour = 6,
                    Minute = 5,
                    Second = 34
                },
                Versions = new List<string> { "KQ V0.1" }
            };
            
            var originalFile = new KQFile(
                new List<KQController>() { controller0, controller1 },
                new List<StandardAxis>() { axis0, axis1}, 
                new List<StandardGroup>() { group0},
                new List<StandardScenery>() { scenery0},
                new List<StandardPatch>() { patch0 },
                new List<StandardCue>() { cue0}
            );
            originalFile.Header = header;

            var serializer = new XmlSerializer(typeof(KQFile));
            string xml;

            // Serialize to string
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, originalFile);
                xml = writer.ToString();
                File.WriteAllText("../../../testKQFile.txt", xml);
            }

            // Deserialize back to object
            KQFile deserialized;
            using (var reader = new StringReader(xml))
            {
                deserialized = (KQFile)serializer.Deserialize(reader);
            }
            deserialized.Should().BeEquivalentTo(originalFile, options => options
                .Excluding(member => member.Name == "CurrentPos")
                .Excluding(member => member.Name == "CurrentVel")
                .Excluding(member => member.Name == "HasAlarm")
                .Excluding(member => member.Name == "IsEnabled"));
        }
    }
}
