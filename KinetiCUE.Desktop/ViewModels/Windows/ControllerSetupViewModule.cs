using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Desktop.ViewModels.Windows
{
    public partial class ControllerSetupViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _controllerName = "Click PLC 1";

        [ObservableProperty]
        private string _ipAddress = "192.168.1.10";

        public Action? RequestClose { get; set; }

        [RelayCommand]
        private void SaveAndClose()
        {
            // TODO: Pass the configuration parameters to your background PLC manager here

            // Close the window
            RequestClose?.Invoke();
        }
    }
}
