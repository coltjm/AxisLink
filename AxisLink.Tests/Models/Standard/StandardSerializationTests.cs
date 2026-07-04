using FluentAssertions;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Extended;
using AxisLink.Core.Models.Standard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xunit;


namespace AxisLink.Tests.Models.Standard
{
    public class StandardSerializationTests
    {
        [Fact]
        public void StandardFile_FullRoundTrip()
        {
            var axis0 = new StandardAxis
            {
                Id = 0,
                Name = "motor 1",
                Type = AxisType.Other,
                Location = new AxisLocation { X = 100, Y = 300 },
                Length = 10,
                Positioning = AxisPositioning.No,
                LowLimit = -10,
                HighLimit = 500,
                SpeedType = AxisSpeedType.Fixed,
                MaxSpeed = 1000,
                MaxAccel = 500,
                MaxDecel = 500,
                MaxLoad = 20
            };
            var axis1 = new StandardAxis
            {
                Id = 1,
                Name = "motor 2",
                Type = AxisType.Rotary,
                Location = new AxisLocation { X = 0, Y = 0 },
                Length = 100,
                Positioning = AxisPositioning.Yes,
                LowLimit = 0,
                HighLimit = 100,
                SpeedType = AxisSpeedType.Variable,
                MaxSpeed = 100,
                MaxAccel = 50,
                MaxDecel = 50,
                MaxLoad = 200

            };
            var group0 = new StandardGroup
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
            var scenery0 = new StandardScenery
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
                        Name = "minimum",
                        Position = 100
                    },
                    HighTrim = new SceneryLimitTrim
                    {
                        Name = "maximum",
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
            var patch0 = new StandardPatch
            {
                // References scenery0
                Id = 0,
                // Connect scenery0 to group0
                GroupId = 0
            };
            var cuePart0 = new StandardCuePart
            {
                Id = 0,
                User = "FOH",
                Playback = 2,
                MoveType = CuePartMoveType.Linear,
                Start = new CuePartStart
                {
                    Type = CuePartStartType.Limit,
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
            var cue0 = new StandardCue
            {
                Number = "1.23",
                Name = "Fly tree in",
                Stack = 2,
                CueParts = new List<StandardCuePart> { cuePart0 }
            };
            var header = new StandardHeader
            {
                ShowName = "StandardTest",
                Notes = "Unit test for App development",
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
                Versions = new List<string> { "AxisLink V0.1" }
            };

            var originalFile = new StandardFile(
                new List<StandardAxis>() { axis0, axis1 },
                new List<StandardGroup>() { group0 },
                new List<StandardScenery>() { scenery0 },
                new List<StandardPatch>() { patch0 },
                new List<StandardCue>() { cue0 }
            );
            originalFile.Header = header;

            var serializer = new XmlSerializer(typeof(StandardFile));
            string xml;

            // Serialize to string
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, originalFile);
                xml = writer.ToString();
                File.WriteAllText("../../../testStandardFile.txt", xml);
            }

            // Deserialize back to object
            StandardFile deserialized;
            using (var reader = new StringReader(xml))
            {
                deserialized = (StandardFile)serializer.Deserialize(reader);
            }
            deserialized.Should().BeEquivalentTo(originalFile, options => options);
        }

    }
    }
