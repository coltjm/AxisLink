using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace AxisLink.Infrastructure.Mappers
{
    /// <summary>
    /// Maps a Click Address in the form X001 to a decimal modbus register
    /// </summary>
    public static class ClickAddressMapper
    {
        static int GetPrefix(string charPrefix) => charPrefix switch
        {
            // Map prefixes to the start index of the memory type
            "X" => 100001,
            "Y" => 8193,
            "C" => 16385,
            "T" => 145057,
            "CT" => 149153,
            "SC" => 161441,
            "DS" => 400001,
            "DD" => 416385,
            "DH" => 424577,
            "DF" => 428673,
            "XD" => 357347,
            "YD" => 457859,
            "TD" => 445057,
            "CTD" => 449153,
            "SD" => 361441,
            "TXT" => 436865,
            _ => -1
        };

        // Only needed for X and Y
        public static int GetModbusBit(int val)
        {
            // Since x and y do a weird dance where it skips x017 and things like that, this adjusts properly
            int slot = val / 100;
            int remaining = val % 100;
            int bank = remaining > 20 ? 1 : 0;
            int point = remaining % 20;

            return (slot * 32) + (bank * 16) + point - 1;
        }

        public static int GetModbusAddress(string clickAddress)
        {
            // NOTE: Use of YD0/YD0u and XD0/0u are currently unsupported because quite frankly i cant be bothered to figure out why they switched to 0 based for 2 memory types
            // Actually i tested it and it works but still... its dumb so dont use it out of principle (im sure theres a good reason i just dont know it)

            int splitIndex = clickAddress.IndexOfAny("0123456789".ToCharArray());

            // split index is used as length here -> doesnt matter since we are going from index 0
            string prefixString = clickAddress[0..splitIndex];
            int prefix = GetPrefix(prefixString);

            // Try to parse numerical part
            int suffix = 0;
            try 
            { 
                suffix = int.Parse(clickAddress[splitIndex..]);
            }
            catch
            {
                // Throw error if an invalid Click Number was passed
                throw new ArgumentException("Invalid Click Number: " + clickAddress[splitIndex..]);
            }

            // Throw error if an invalid Click Address was passed
            if(prefix == -1)
            {
                throw new ArgumentException("Invalid Click Prefix: "+ prefixString);

            }

            // Handle all the fun and unique ways that memory types translate to addresses!
            int modbusAddress;
            // Weird formula - reminant from old tech that carried over i believe. sets of 16 bits with gaps between
            if(prefixString == "X" || prefixString == "Y") 
            {
                modbusAddress = prefix + GetModbusBit(suffix);
            }
            // These each take two spaces
            else if(prefixString == "DD" || prefixString == "DF" || prefixString == "XD" || prefixString == "YD" || prefixString == "CTD")
            {
                modbusAddress = prefix + 2*(suffix-1);
            }
            // These each take half a space?? not sure why but they do
            else if(prefixString == "TXT")
            {
                modbusAddress = prefix + (suffix - 1)/2;
            }
            // Normal 1 to 1 mapping
            else
            {
                modbusAddress = prefix + suffix-1;
            }
            return modbusAddress;
        }
    }
}
