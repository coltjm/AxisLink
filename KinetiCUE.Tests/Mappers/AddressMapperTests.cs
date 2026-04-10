using System;
using System.Collections.Generic;
using System.Text;
using KinetiCUE.Infrastructure.Mappers;

namespace KinetiCUE.Tests.Mappers
{
    public class AddressMapperTests
    {
        // Exported data from Click PLC to test against a sample of
        public static IEnumerable<object[]> AddressTestData => new List<object[]>
        {
            // --- X Inputs (Testing Slot 0 Bank 1 & 2) ---
            new object[] { "X001", 100001 }, new object[] { "X008", 100008 }, new object[] { "X016", 100016 },
            new object[] { "X021", 100017 }, new object[] { "X028", 100024 }, new object[] { "X036", 100032 },
            // --- X Inputs (Testing Slot 1 & 2 jumps) ---
            new object[] { "X101", 100033 }, new object[] { "X116", 100048 },
            new object[] { "X201", 100065 }, new object[] { "X216", 100080 },
            new object[] { "X801", 100257 }, new object[] { "X816", 100272 },

            // --- Y Outputs (Testing Slot 0 Bank 1 & 2) ---
            new object[] { "Y001", 8193 }, new object[] { "Y008", 8200 }, new object[] { "Y016", 8208 },
            new object[] { "Y021", 8209 }, new object[] { "Y028", 8216 }, new object[] { "Y036", 8224 },
            // --- Y Outputs (Testing Slot 1 & 4 jumps) ---
            new object[] { "Y101", 8225 }, new object[] { "Y116", 8240 },
            new object[] { "Y401", 8321 }, new object[] { "Y416", 8336 },
            new object[] { "Y815", 8463 }, new object[] { "Y816", 8464 },

            // --- C Relays (Linear mapping) ---
            new object[] { "C1", 16385 }, new object[] { "C16", 16400 }, new object[] { "C17", 16401 },
            new object[] { "C100", 16484 }, new object[] { "C500", 16884 }, new object[] { "C1000", 17384 },
            new object[] { "C1500", 17884 }, new object[] { "C2000", 18384 },

            // --- T Timers (Linear mapping) ---
            new object[] { "T1", 145057 }, new object[] { "T36", 145092 }, new object[] { "T100", 145156 },
            new object[] { "T250", 145306 }, new object[] { "T500", 145556 },

            // --- CT Counters (Linear mapping) ---
            new object[] { "CT1", 149153 }, new object[] { "CT50", 149202 }, new object[] { "CT100", 149252 },
            new object[] { "CT250", 149402 },

            // --- SC System Control Bits (Linear mapping) ---
            new object[] { "SC1", 161441 }, new object[] { "SC10", 161450 }, new object[] { "SC28", 161468 },
            new object[] { "SC100", 161540 }, new object[] { "SC500", 161940 }, new object[] { "SC1000", 162440 },

            // --- DS Data Single (Linear mapping 4xxxx) ---
            new object[] { "DS1", 400001 }, new object[] { "DS10", 400010 }, new object[] { "DS100", 400100 },
            new object[] { "DS500", 400500 }, new object[] { "DS1000", 401000 }, new object[] { "DS1500", 401500 },
            new object[] { "DS2000", 402000 }, new object[] { "DS2500", 402500 }, new object[] { "DS3000", 403000 },
            new object[] { "DS3500", 403500 }, new object[] { "DS4000", 404000 }, new object[] { "DS4500", 404500 },

            // --- DF Data Float (Double-wide registers) ---
            new object[] { "DF1", 428673 }, new object[] { "DF2", 428675 }, new object[] { "DF3", 428677 },
            new object[] { "DF50", 428771 }, new object[] { "DF100", 428871 }, new object[] { "DF250", 429171 },
            new object[] { "DF500", 429671 },

            // --- DD Data Double (Double-wide registers) ---
            new object[] { "DD1", 416385 }, new object[] { "DD2", 416387 }, new object[] { "DD100", 416583 },
            new object[] { "DD250", 416883 }, new object[] { "DD400", 417183 },

            // --- DH Data Hex (Double-wide registers) ---
            new object[] { "DH1", 424577 }, new object[] { "DH2", 424578 }, new object[] { "DH100", 424676 },
            new object[] { "DH500", 425076 },

            // --- TXT Text (Single register per 2 chars) ---
            new object[] { "TXT1", 436865 }, new object[] { "TXT2", 436865 }, new object[] { "TXT3", 436866 },
            new object[] { "TXT100", 436914 }, new object[] { "TXT500", 437114 }, new object[] { "TXT1000", 437364 },

            // --- TD/XD/YD Double Word Bits ---
            new object[] { "XD0", 357345 }, new object[] { "XD8", 357361 },
            new object[] { "YD0", 457857 }, new object[] { "YD8", 457873 },
            new object[] { "TD1", 445057 }, new object[] { "TD100", 445156 }
        };

        [Theory]
        [MemberData(nameof(AddressTestData))]
        public void GetModbusAddress_BulkTest(string clickAddr, int expectedModbus)
        {
            Assert.Equal(expectedModbus, ClickAddressMapper.GetModbusAddress(clickAddr));
        }
    }
}
