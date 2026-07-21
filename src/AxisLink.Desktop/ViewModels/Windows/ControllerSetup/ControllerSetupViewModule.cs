using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AxisLink.Core.Management;
using System;
using System.Collections.Generic;
using System.Text;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Extended;
using System.Diagnostics;

namespace AxisLink.Desktop.ViewModels.Windows.ControllerSetup
{
    public partial class ControllerSetupViewModel : ViewModelBase
    {
        private readonly ShowFileManager FileManager;

        private readonly MotionManager motionManager;

        [ObservableProperty]
        private ModbusControllerSetupViewModel _modbusControllerSetupViewModel = new ModbusControllerSetupViewModel();

        public Action? RequestClose { get; set; }

        [RelayCommand]
        private void SaveAndClose()
        {
            try
            {
                // TODO: Set connection config based on the selected controller type
                ConnectionConfig connectionConfig = new ModbusConfig(ModbusControllerSetupViewModel.IpAddress,
                    ModbusControllerSetupViewModel.Port.ToString(),
                    ModbusControllerSetupViewModel.EstopAddress);
                ExtendedController controller = new ExtendedController(
                    id: motionManager.nextControllerId,
                    name: ModbusControllerSetupViewModel.ControllerName,
                    config: connectionConfig);
                motionManager.AddNewController(controller);
                // Close the window
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        public ControllerSetupViewModel(ShowFileManager fileManager, MotionManager _motionManager)
        {
            FileManager = fileManager;
            motionManager = _motionManager;
            // Initialize the ModbusControllerSetupViewModel with default values
            _modbusControllerSetupViewModel = new ModbusControllerSetupViewModel();
        }
    }
}
