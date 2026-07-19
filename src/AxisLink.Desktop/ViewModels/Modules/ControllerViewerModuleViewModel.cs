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
    public partial class ControllerViewerModuleViewModel : ModuleViewModelBase
    {
        private readonly ShowFileManager FileManager;
        private readonly MotionManager motionManager;
        private readonly IConsoleLogger Logger;
        // List of controllers to display in the module, bound to the UI
        public ObservableCollection<ExtendedController> DisplayControllers { get; } = new();
        public ControllerViewerModuleViewModel(ShowFileManager fileManager, MotionManager _motionManager, IConsoleLogger logger)
        {
            FileManager = fileManager;
            motionManager = _motionManager;
            Logger = logger;
            Title = "Controller Viewer";
            // Initialize the DisplayControllers collection with the current controllers from the MotionManager
            foreach (var controller in motionManager.Controllers)
            {
                DisplayControllers.Add(controller);
            }
            // Subscribe to events for when controllers are added or removed
            motionManager.ControllerAdded += OnControllerAdded;
            motionManager.ControllerRemoved += OnControllerRemoved;
        }

        private void OnControllerAdded(ExtendedController controller)
        {
            // When controller is added, update UI collection on the UI thread
            Avalonia.Threading.Dispatcher.UIThread.Post(() => DisplayControllers.Add(controller));
        }

        private void OnControllerRemoved(ExtendedController controller)
        {
            // When controller is removed, update UI collection on the UI thread
            Avalonia.Threading.Dispatcher.UIThread.Post(() => DisplayControllers.Remove(controller));
        }

        // Clean up event handlers when the module panel is closed/destroyed
        public override void Dispose()
        {
            motionManager.ControllerAdded -= OnControllerAdded;
            motionManager.ControllerRemoved -= OnControllerRemoved;
            base.Dispose();
        }

    }
}
