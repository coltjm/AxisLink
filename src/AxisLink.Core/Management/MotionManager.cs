using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Show;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AxisLink.Core.Management
{
    public class MotionManager
    {
        // Dictionary to hold motion services for each controller, keyed by controller ID
        private readonly Dictionary<int, IMotionService> _services = [];
        // ShowFileManager to manage the current show file and its machinery
        private readonly ShowFileManager _showFileManager;
        // MotionServiceFactory to create motion services based on controller configurations
        private readonly IMotionServiceFactory _serviceFactory;
        public readonly List<Axis> Axes;
        public readonly List<Controller> Controllers;
        // Events to notify when axes or controllers are added or removed
        public event Action<Axis>? AxisAdded;
        public event Action<Axis>? AxisRemoved;
        public event Action<Controller>? ControllerAdded;
        public event Action<Controller>? ControllerRemoved;
        // Running counters for the next axis and controller IDs
        public int nextAxisId { get; private set; }
        public int nextControllerId { get; private set; }
        public MotionManager(ShowFileManager showFileManager, IMotionServiceFactory serviceFactory)
        {
            // Initialize the ShowFileManager and MotionServiceFactory from dependency injection
            _showFileManager = showFileManager;
            _serviceFactory = serviceFactory;
            Axes = _showFileManager?.CurrentShow?.Machinery?.Axes;
            Controllers = _showFileManager?.CurrentShow?.Controllers;
            // Set the next IDs based on the maximum existing IDs in the lists, or start from 1 if the lists are empty
            nextAxisId = showFileManager.CurrentShow.Machinery.Axes.Count != 0 ? Axes.Max(a => a.Id) + 1 : 1;
            nextControllerId = Controllers.Count != 0 ? Controllers.Max(c => c.Id) + 1 : 1;
        }

        // Startup motion services for all controllers in the show file
        public async Task StartupAsync()
        {
            // Clear existing services for startup
            _services.Clear();

            // Go through all controllers
            foreach (var controller in Controllers)
            {
                try
                {
                    // Create appropriate motion service for the controller based on its configuration
                    IMotionService service = _serviceFactory.CreateService(controller.Config);

                    // Use controller id as key for dictionary
                    int key = controller.Id;

                    // Add the service to the dictionary and connect
                    _services.Add(key, service);
                    await service.ConnectAsync();
                }
                catch (Exception ex)
                {
                    // Log error and continue
                    System.Diagnostics.Debug.WriteLine($"Failed to load controller {controller.Id}: {ex.Message}");
                }
            }
        }

        public async Task RefreshAsync()
        {
            if (Controllers == null) return;

            var fileIds = Controllers.Select(c => c.Id).ToHashSet();
            // Remove service if controller was removed from the show file
            var deletedIds = _services.Keys.Where(id => !fileIds.Contains(id)).ToList();
            foreach (var id in deletedIds)
            {
                try
                {
                    await _services[id].DisconnectAsync();
                }
                finally
                {
                    _services.Remove(id);
                }
            }
            // go through all controllers in the file
            foreach (var controller in Controllers)
            {
                // If its not in the dictionary, add it and connect
                if (!_services.ContainsKey(controller.Id))
                {
                    try
                    {
                        IMotionService service = _serviceFactory.CreateService(controller.Config);
                        _services.Add(controller.Id, service);
                        await service.ConnectAsync();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to dynamically add controller {controller.Id}: {ex.Message}");
                    }
                }
            }
        }
        
        public void AddNewAxis(Axis axis)
        {
            ArgumentNullException.ThrowIfNull(axis);
            // update show file and next id accordingly
            Axes.Add(axis);
            nextAxisId++;
            // Trigger event to notify listeners of the new axis
            AxisAdded?.Invoke(axis);
        }
        public void AddNewController(Controller controller)
        {
            ArgumentNullException.ThrowIfNull(controller);
            // Add controller and update show file and next id accordingly
            Controllers.Add(controller);
            nextControllerId++;
            // Trigger event to notify listeners of the new controller
            ControllerAdded?.Invoke(controller);
        }

        public void RemoveController(Controller controller) {
            ArgumentNullException.ThrowIfNull(controller);
            // Remove controller and update show file accordingly
            // Do not update next id, skip over any removed ids to avoid conflicts
            Controllers.Remove(controller);
            // Trigger event to notify listeners of the removed controller
            ControllerRemoved?.Invoke(controller);  
        }

        public void RemoveAxis(Axis axis)
        {
            ArgumentNullException.ThrowIfNull(axis);
            // Remove axis and update show file accordingly
            // Do not update next id, skip over any removed ids to avoid conflicts
            Axes.Remove(axis);
            // Trigger event to notify listeners of the removed axis
            AxisRemoved?.Invoke(axis);
        }
    }
}
