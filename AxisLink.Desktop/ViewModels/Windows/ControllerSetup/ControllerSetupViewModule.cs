using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AxisLink.Core.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Windows.ControllerSetup
{
    public partial class ControllerSetupViewModel : ViewModelBase
    {
        private readonly ShowFileManager FileManager;

        [ObservableProperty]
        private ModbusControllerSetupViewModel _modbusControllerSetupViewModel = new ModbusControllerSetupViewModel();

        public Action? RequestClose { get; set; }

        [RelayCommand]
        private void SaveAndClose()
        {
            // TODO: Pass the configuration parameters to your background PLC manager here

            // Close the window
            RequestClose?.Invoke();
        }

        public ControllerSetupViewModel(ShowFileManager fileManager)
        {
            FileManager = fileManager;
            // Initialize the ModbusControllerSetupViewModel with default values
            _modbusControllerSetupViewModel = new ModbusControllerSetupViewModel();
        }
    }
}
