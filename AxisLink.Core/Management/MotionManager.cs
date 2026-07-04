using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Management
{
    public class MotionManager
    {
        private readonly Dictionary<int, IMotionService> _services = new();
        private readonly ShowFileManager _showFileManager;
        private readonly IMotionServiceFactory _serviceFactory;
        public MotionManager(ShowFileManager showFileManager, IMotionServiceFactory serviceFactory)
        {
            // Initialize the ShowFileManager and MotionServiceFactory from dependency injection
            _showFileManager = showFileManager;
            _serviceFactory = serviceFactory;
        }

        // Startup motion services for all controllers in the show file
        public async Task StartupAsync()
        {
            // Clear existing services for startup
            _services.Clear();

            // Go through all controllers
            foreach (var controller in _showFileManager.CurrentShow.Controllers)
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
            if (_showFileManager.CurrentShow?.Controllers == null) return;

            var currentFileControllers = _showFileManager.CurrentShow.Controllers;
            var fileIds = currentFileControllers.Select(c => c.Id).ToHashSet();
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
            foreach (var controller in currentFileControllers)
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
        

        
        
    }
}
