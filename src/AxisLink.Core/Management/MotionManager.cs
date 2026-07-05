using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AxisLink.Core.Management
{
    public class MotionManager
    {
        private readonly Dictionary<int, IMotionService> _services = new();
        private readonly ShowFileManager _showFileManager;
        private readonly IMotionServiceFactory _serviceFactory;
        public List<ExtendedAxis> Axes { get; private set; } = new();
        public List<ExtendedController> Controllers { get; private set; } = new();
        public event Action<ExtendedAxis>? AxisAdded;
        public event Action<ExtendedAxis>? AxisRemoved;
        public event Action<ExtendedController>? ControllerAdded;
        public event Action<ExtendedController>? ControllerRemoved;
        public int nextAxisId { get; private set; }
        public int nextControllerId { get; private set; }
        public MotionManager(ShowFileManager showFileManager, IMotionServiceFactory serviceFactory)
        {
            // Initialize the ShowFileManager and MotionServiceFactory from dependency injection
            _showFileManager = showFileManager;
            _serviceFactory = serviceFactory;
            Axes = _showFileManager.CurrentShow?.Machinery?.Axes?
                .OfType<ExtendedAxis>().ToList() ?? new List<ExtendedAxis>();
            Controllers = _showFileManager.CurrentShow?.Controllers?
                .OfType<ExtendedController>().ToList() ?? new List<ExtendedController>();
            nextAxisId = Axes.Any() ? Axes.Max(a => a.Id) + 1 : 1;
            nextControllerId = Controllers.Any() ? Controllers.Max(c => c.Id) + 1 : 1;
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
        
        public void AddNewAxis(ExtendedAxis axis)
        {
            if (axis == null) throw new ArgumentNullException(nameof(axis));
            Axes.Add(axis);
            _showFileManager.CurrentShow.Machinery?.Axes?.Add(axis);
            nextAxisId++;
            AxisAdded?.Invoke(axis);
        }
        public void AddNewController(ExtendedController controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            Controllers.Add(controller);
            _showFileManager.CurrentShow.Controllers.Add(controller);
            nextControllerId++;
            ControllerAdded?.Invoke(controller);
        }

        public void RemoveController(ExtendedController controller) {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            Controllers.Remove(controller);
            _showFileManager.CurrentShow.Controllers.Remove(controller);
            ControllerRemoved?.Invoke(controller);  
        }

        public void RemoveAxis(ExtendedAxis axis)
        {
            if (axis == null) throw new ArgumentNullException(nameof(axis));
            Axes.Remove(axis);
            _showFileManager.CurrentShow.Machinery?.Axes?.Remove(axis);
            AxisRemoved?.Invoke(axis);
        }
    }
}
