using Avalonia.Controls;
using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.ViewModels;
using AxisLink.Desktop.ViewModels.Modules;
using Dock.Model.Controls;
using Dock.Model.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AxisLink.Desktop.Utilities
{
    public partial class WorkspaceManager
    {
        private readonly IConsoleLogger _logger;
        private readonly IServiceProvider _serviceProvider;
        public ObservableCollection<ModuleViewModelBase> ActiveModules { get; } = new();

        public WorkspaceManager(IConsoleLogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            InitializeDefaultWorkspace();
        }

        private void InitializeDefaultWorkspace()
        {
                
            var loggerModule = new LoggerModuleViewModel(_logger) { Title = "System Log Console" };
            ActiveModules.Add(loggerModule);

        }

        public void ShowAxisViewer()
        {
            var module = _serviceProvider.GetRequiredService<AxisViewerModuleViewModel>();
            ActiveModules.Add(module);
        }

        public void ShowCueList()
        {
            var module = _serviceProvider.GetRequiredService<CueListModuleViewModel>();
            ActiveModules.Add(module);
        }

        public void ShowControllerViewer()
        {
            var module = _serviceProvider.GetRequiredService<ControllerViewerModuleViewModel>();
            ActiveModules.Add(module);
        }

        public void ShowCueStack()
        {
            var module = _serviceProvider.GetRequiredService<CueStackModuleViewModel>();
            ActiveModules.Add(module);
        }

        public void ShowLogger()
        {
            var module = _serviceProvider.GetRequiredService<LoggerModuleViewModel>();
            ActiveModules.Add(module);
        }
    }
}
