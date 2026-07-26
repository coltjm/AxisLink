using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Windows.AxisSetup
{
    public partial class AxisSetupViewModel : ViewModelBase
    {
        private readonly ShowFileManager FileManager;
        private readonly MotionManager motionManager;
        private readonly IConsoleLogger Logger;
        [ObservableProperty]
        private Axis createdAxis;
        [ObservableProperty]
        private bool _isOneToOneController = true;

        [ObservableProperty]
        private string _protocolConfigNotes = string.Empty;

        [ObservableProperty]
        private float? _homePosition;

        [ObservableProperty]
        private object? _selectedController;

        [ObservableProperty]
        private bool _hasPositioning = true;

        [ObservableProperty]
        private int _selectedMotorTypeIndex = 0;

        public ObservableCollection<object> AvailableControllers { get; } = new();
        public ObservableCollection<string> AvailableSensors { get; } = new();
        public Action? RequestClose { get; set; }



        public AxisSetupViewModel(ShowFileManager fileManager, MotionManager _motionManager, IConsoleLogger logger)
        {
            FileManager = fileManager;
            motionManager = _motionManager;
            Logger = logger;
            createdAxis = new Axis{ Id = motionManager.nextAxisId, Name=$"Axis {motionManager.nextAxisId}" };
        }

        [RelayCommand]
        private void SaveAxis()
        {
            Logger.LogInfo("Saving Axis...");
            // TODO: Pass the configuration parameters to controller manager here
            if(CreatedAxis == null)
            {
                // Log error or handle the case where createdAxis is null
                return;
            }
            motionManager.AddNewAxis(CreatedAxis);
            Logger.LogInfo("Added Axis: "+CreatedAxis.Name);
            // Close the window
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            // Close the window
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void CreateNewSensor()
        {
            // Logic to open Sensor Creation modal window
        }

    }
}
