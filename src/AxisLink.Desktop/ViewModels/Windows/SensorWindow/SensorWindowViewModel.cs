using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Windows.SensorsWindow
{
    public partial class SensorWindowViewModel : ViewModelBase
    {
        private readonly ShowFileManager FileManager;
        private readonly MotionManager motionManager;
        private readonly IConsoleLogger Logger;
        [ObservableProperty]
        private Sensor _createdSensor;

        [ObservableProperty]
        private string _protocolConfigNotes = string.Empty;

        [ObservableProperty]
        private SensorTypes _sensorType;
        [ObservableProperty]
        private int _selectedSensorTypeIndex = 0;
        [ObservableProperty]
        private object? _selectedController;


        public ObservableCollection<object> AvailableControllers { get; } = new();
        public Action? RequestClose { get; set; }



        public SensorWindowViewModel(ShowFileManager fileManager, MotionManager _motionManager, IConsoleLogger logger)
        {
            FileManager = fileManager;
            motionManager = _motionManager;
            Logger = logger;
            CreatedSensor = new Sensor{ Id= 1};
        }

        [RelayCommand]
        private void SaveSensor()
        {
            Logger.LogInfo("Saving Sensor...");
            // TODO: Pass the configuration parameters to controller manager here
            
            // Close the window
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            // Close the window
            RequestClose?.Invoke();
        }

    }
}
