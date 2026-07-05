using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Extended;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Windows.AxisSetup
{
    public partial class AxisSetupViewModel : ViewModelBase
    {
        private readonly ShowFileManager FileManager;
        private readonly MotionManager motionManager;
        private readonly IConsoleLogger Logger;
        [ObservableProperty]
        private ExtendedAxis createdAxis;

        public Action? RequestClose { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanGoBack))]
        [NotifyPropertyChangedFor(nameof(IsLastPage))]
        [NotifyPropertyChangedFor(nameof(Page0FontWeight))]
        [NotifyPropertyChangedFor(nameof(Page1FontWeight))]
        [NotifyPropertyChangedFor(nameof(Page2FontWeight))]
        [NotifyPropertyChangedFor(nameof(Page0Foreground))]
        [NotifyPropertyChangedFor(nameof(Page1Foreground))]
        [NotifyPropertyChangedFor(nameof(Page2Foreground))]
        private int _currentPageIndex = 0;
        public bool CanGoBack => CurrentPageIndex > 0;
        public bool IsLastPage => CurrentPageIndex == 2;

        // Visual Status Bar Tracking Computations
        public string Page0FontWeight => CurrentPageIndex == 0 ? "Bold" : "Normal";
        public string Page1FontWeight => CurrentPageIndex == 1 ? "Bold" : "Normal";
        public string Page2FontWeight => CurrentPageIndex == 2 ? "Bold" : "Normal";
        public string Page0Foreground => CurrentPageIndex == 0 ? "#007ACC" : "Gray";
        public string Page1Foreground => CurrentPageIndex == 1 ? "#007ACC" : "Gray";
        public string Page2Foreground => CurrentPageIndex == 2 ? "#007ACC" : "Gray";

        [RelayCommand]
        private void NextPage() { if (CurrentPageIndex < 2) CurrentPageIndex++; }

        [RelayCommand]
        private void PreviousPage() { if (CurrentPageIndex > 0) CurrentPageIndex--; }

        public AxisSetupViewModel(ShowFileManager fileManager, MotionManager _motionManager, IConsoleLogger logger)
        {
            FileManager = fileManager;
            motionManager = _motionManager;
            Logger = logger;
            createdAxis = new ExtendedAxis{ Id = motionManager.nextAxisId };
        }

        [RelayCommand]
        private void SaveAxis()
        {
            Logger.LogInfo("Saving Axis...");
            // TODO: Pass the configuration parameters to your background PLC manager here
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

    }
}
