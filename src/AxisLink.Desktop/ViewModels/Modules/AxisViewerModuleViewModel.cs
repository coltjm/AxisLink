using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
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
        // List of axes to display in the module, bound to the UI
        public ObservableCollection<Axis> DisplayAxes { get; } = new();
        public AxisViewerModuleViewModel(ShowFileManager fileManager, MotionManager _motionManager, IConsoleLogger logger)
        {
            FileManager = fileManager;
            motionManager = _motionManager;
            Logger = logger;
            Title = "Motor Axis Viewer";
            // Initialize the DisplayAxes collection with the current axes from the MotionManager
            foreach (var axis in motionManager.Axes)
            {
                DisplayAxes.Add(axis);
            }
            // Subscribe to events for when axes are added or removed
            motionManager.AxisAdded += OnAxisAdded;
            motionManager.AxisRemoved += OnAxisRemoved;
        }

        private void OnAxisAdded(Axis axis)
        {
            // When axis is added, update UI collection on the UI thread
            Avalonia.Threading.Dispatcher.UIThread.Post(() => DisplayAxes.Add(axis));
        }

        private void OnAxisRemoved(Axis axis)
        {
            // When axis is removed, update UI collection on the UI thread
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
