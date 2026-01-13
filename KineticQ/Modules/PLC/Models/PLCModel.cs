using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KinetiCUE.Modules.PLC.Models
{
    public partial class PLCModel : ObservableObject
    {
        // Saved Configuration
        [ObservableProperty] private int _id = 0;
        [ObservableProperty] private string _name = "Main PLC";
        [ObservableProperty] private string _ipAddress = "192.168.0.10";
        [ObservableProperty] private int _port = 502;
        [ObservableProperty] private int _unitId = 0;
        [ObservableProperty] private int _eStopSignalAddress = 4;
        // Runtime Status (Do not save to disk)
        [JsonIgnore]
        [ObservableProperty]
        private bool _isConnected;
        [ObservableProperty]
        private bool _isEStopped;

        public string StatusDisplayString => $"{Name.ToUpper()}: {(IsConnected ? "CONNECTED" : "DISCONNECTED")}";

        public PLCModel(int id, string name, string ip, int port, int unitId, int estopSignalAddress) 
            { 
            Id = id;
            Name = name;
            IpAddress = ip;
            Port = port;
            UnitId = unitId;
            EStopSignalAddress = estopSignalAddress;
            
            }

        public PLCModel()
        {
        }

    }
}
