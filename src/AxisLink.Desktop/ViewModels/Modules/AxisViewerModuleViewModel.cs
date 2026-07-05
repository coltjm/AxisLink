using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Extended;
using AxisLink.Core.Models.Standard;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public partial class AxisViewerModuleViewModel : ModuleViewModelBase
    {
        private readonly ShowFileManager FileManager;
        private readonly MotionManager motionManager;
        private readonly IConsoleLogger Logger;
        public ObservableCollection<ExtendedAxis> DisplayAxes { get; } = new();
        public AxisViewerModuleViewModel(ShowFileManager fileManager, MotionManager _motionManager, IConsoleLogger logger)
        {
            FileManager = fileManager;
            motionManager = _motionManager;
            Logger = logger;
            Title = "Motor Axis Viewer";
            foreach (var axis in motionManager.Axes)
            {
                DisplayAxes.Add(axis);
            }

            motionManager.AxisAdded += OnAxisAdded;
            motionManager.AxisRemoved += OnAxisRemoved;
        }

        private void OnAxisAdded(ExtendedAxis axis)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => DisplayAxes.Add(axis));
        }

        private void OnAxisRemoved(ExtendedAxis axis)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => DisplayAxes.Remove(axis));
        }

        // Clean up event handlers when the module panel is closed/destroyed
        public override void Dispose()
        {
            motionManager.AxisAdded -= OnAxisAdded;
            motionManager.AxisRemoved -= OnAxisRemoved;
            base.Dispose();
        }

    }
}
