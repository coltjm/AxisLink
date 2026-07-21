using Avalonia.Controls;
using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.ViewModels;
using AxisLink.Desktop.ViewModels.Modules;
using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Avalonia.Controls;
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
    public partial class WorkspaceManager : ObservableObject
    {
        private readonly IConsoleLogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFactory _dockFactory;

        [ObservableProperty]
        private IRootDock? _rootLayout;

        // Collection of the currently active modules in the workspace
        public ObservableCollection<ModuleViewModelBase> ActiveModules { get; } = new();

        public WorkspaceManager(IConsoleLogger logger, IServiceProvider serviceProvider, IFactory dockFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dockFactory = dockFactory;

            RootLayout = CreateDefaultWorkspaceLayout();
        }

        private IRootDock CreateDefaultWorkspaceLayout()
        {
            var cueListModule = _serviceProvider.GetRequiredService<CueListModuleViewModel>();
            cueListModule.Id = "CueListMaster";
            cueListModule.Title = "Cue List";

            var axisViewerModule = _serviceProvider.GetRequiredService<AxisViewerModuleViewModel>();
            axisViewerModule.Id = "AxisViewerVelocity";
            axisViewerModule.Title = "Axis Viewer - Velocity";

            var controllerViewerModule = _serviceProvider.GetRequiredService<ControllerViewerModuleViewModel>();
            controllerViewerModule.Id = "ControllerViewerMaster";

            var loggerModule = _serviceProvider.GetRequiredService<LoggerModuleViewModel>();
            loggerModule.Id = "LoggerConsole";

            // 1. STRICT FIX: Use Factory methods to create docks instead of 'new'
            var mainDocumentDock = _dockFactory.CreateDocumentDock();
            mainDocumentDock.Id = "MainDocuments";
            mainDocumentDock.IsCollapsable = false;
            mainDocumentDock.ActiveDockable = cueListModule;
            mainDocumentDock.VisibleDockables = _dockFactory.CreateList<IDockable>(cueListModule, axisViewerModule);

            var rightToolDock = _dockFactory.CreateToolDock();
            rightToolDock.Id = "RightTools";
            rightToolDock.ActiveDockable = controllerViewerModule;
            rightToolDock.VisibleDockables = _dockFactory.CreateList<IDockable>(controllerViewerModule);

            var bottomToolDock = _dockFactory.CreateToolDock();
            bottomToolDock.Id = "BottomTools";
            bottomToolDock.ActiveDockable = loggerModule;
            bottomToolDock.VisibleDockables = _dockFactory.CreateList<IDockable>(loggerModule);

            var mainHorizontalGroup = _dockFactory.CreateProportionalDock();
            mainHorizontalGroup.Orientation = Orientation.Horizontal;
            mainHorizontalGroup.VisibleDockables = _dockFactory.CreateList<IDockable>(
                mainDocumentDock,
                _dockFactory.CreateProportionalDockSplitter(),
                rightToolDock
            );

            var mainVerticalGroup = _dockFactory.CreateProportionalDock();
            mainVerticalGroup.Orientation = Orientation.Vertical;
            mainVerticalGroup.VisibleDockables = _dockFactory.CreateList<IDockable>(
                mainHorizontalGroup,
                _dockFactory.CreateProportionalDockSplitter(),
                bottomToolDock
            );

            var root = _dockFactory.CreateRootDock();
            root.Id = "Root";
            root.IsCollapsable = false;
            root.ActiveDockable = mainVerticalGroup;
            root.DefaultDockable = mainVerticalGroup;
            root.VisibleDockables = _dockFactory.CreateList<IDockable>(mainVerticalGroup);

            // 2. STRICT FIX: Map the modules BEFORE calling InitLayout
            // This stops Dock from wiping your module contexts to null
            _dockFactory.ContextLocator = new Dictionary<string, Func<object>>
            {
                [cueListModule.Id] = () => cueListModule,
                [axisViewerModule.Id] = () => axisViewerModule,
                [controllerViewerModule.Id] = () => controllerViewerModule,
                [loggerModule.Id] = () => loggerModule
            };

            root.Factory = _dockFactory;
            _dockFactory.InitLayout(root);

            return root;
        }

        /// <summary>
        /// Always resolves a new transient instance from DI and appends it to the central document panel,
        /// enabling duplicate views (e.g., multiple AxisViewers).
        /// </summary>
        public void SpawnNewModuleTab<T>(string? customTitle = null) where T : ModuleViewModelBase
        {
            if (_dockFactory.FindDockable(RootLayout, d => d.Id == "MainDocuments") is not IDocumentDock documentDock)
                return;

            var newModule = _serviceProvider.GetRequiredService<T>();
            newModule.Id = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(customTitle))
            {
                newModule.Title = customTitle;
            }

            _dockFactory.AddDockable(documentDock, newModule);
            newModule.Context = newModule;
            _dockFactory.SetActiveDockable(newModule);
        }

        public void ShowAxisViewer() => SpawnNewModuleTab<AxisViewerModuleViewModel>("Axis Viewer");
        public void ShowCueList() => SpawnNewModuleTab<CueListModuleViewModel>("Cue List");
        public void ShowControllerViewer() => SpawnNewModuleTab<ControllerViewerModuleViewModel>("Controllers");
        public void ShowCueStack() => SpawnNewModuleTab<CueStackModuleViewModel>("Cue Stack");
        public void ShowLogger() => SpawnNewModuleTab<LoggerModuleViewModel>("System Log");
    }
}
