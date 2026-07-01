using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Desktop.ViewModels.Windows.ControllerSetup
{
    public partial class ModbusControllerSetupViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _controllerName = "Click PLC 1";

        [ObservableProperty]
        private string _ipAddress = "192.168.1.10";
    }
}
