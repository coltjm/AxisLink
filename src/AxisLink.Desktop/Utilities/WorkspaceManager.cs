using Avalonia.Controls;
using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.ViewModels;
using AxisLink.Desktop.ViewModels.Modules;
using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Avalonia.Controls;
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
            var cueListVm = _serviceProvider.GetRequiredService<CueListModuleViewModel>();
            cueListVm.Title = "Cue List";
            var cueListModule = _dockFactory.CreateDocument();
            cueListModule.Id = "CueListMaster";
            cueListModule.Title = cueListVm.Title;
            cueListModule.Context = cueListVm;

            var controllerViewerVm = _serviceProvider.GetRequiredService<ControllerViewerModuleViewModel>();
            controllerViewerVm.Title = "Controller Viewer - Master";
            var controllerViewerModule = _dockFactory.CreateDocument();
            controllerViewerModule.Id = "ControllerViewerMaster";
            controllerViewerModule.Title = controllerViewerVm.Title;
            controllerViewerModule.Context = controllerViewerVm;

            var sceneryViewerVm = _serviceProvider.GetRequiredService<SceneryViewerModuleViewModel>();
            sceneryViewerVm.Title = "Scenery Viewer";
            var sceneryViewerModule = _dockFactory.CreateDocument();
            sceneryViewerModule.Id = "SceneryViewerMaster";
            sceneryViewerModule.Title = sceneryViewerVm.Title;
            sceneryViewerModule.Context = sceneryViewerVm;

            var loggerVm = _serviceProvider.GetRequiredService<LoggerModuleViewModel>();
            loggerVm.Title = "Logger Console";
            var loggerModule = _dockFactory.CreateDocument();
            loggerModule.Id = "LoggerConsole";
            loggerModule.Title = loggerVm.Title;
            loggerModule.Context = loggerVm;

            var mainDocumentDock = _dockFactory.CreateDocumentDock();
            mainDocumentDock.Id = "MainDocuments";
            mainDocumentDock.IsCollapsable = false;
            mainDocumentDock.ActiveDockable = cueListModule;
            mainDocumentDock.VisibleDockables = _dockFactory.CreateList<IDockable>(cueListModule);

            var rightDocumentDock = _dockFactory.CreateDocumentDock();
            rightDocumentDock.Id = "RightTools";
            rightDocumentDock.ActiveDockable = sceneryViewerModule;
            rightDocumentDock.VisibleDockables = _dockFactory.CreateList<IDockable>(sceneryViewerModule);

            var bottomDocumentDock = _dockFactory.CreateDocumentDock();
            bottomDocumentDock.Id = "BottomTools";
            bottomDocumentDock.ActiveDockable = loggerModule;
            bottomDocumentDock.VisibleDockables = _dockFactory.CreateList<IDockable>(loggerModule);

            var mainHorizontalGroup = _dockFactory.CreateProportionalDock();
            mainHorizontalGroup.Orientation = Orientation.Horizontal;
            mainHorizontalGroup.VisibleDockables = _dockFactory.CreateList<IDockable>(
                mainDocumentDock,
                _dockFactory.CreateProportionalDockSplitter(),
                rightDocumentDock
            );

            var mainVerticalGroup = _dockFactory.CreateProportionalDock();
            mainVerticalGroup.Orientation = Orientation.Vertical;
            mainVerticalGroup.VisibleDockables = _dockFactory.CreateList<IDockable>(
                mainHorizontalGroup,
                _dockFactory.CreateProportionalDockSplitter(),
                bottomDocumentDock
            );

            var root = _dockFactory.CreateRootDock();
            root.Id = "Root";
            root.IsCollapsable = false;
            root.ActiveDockable = mainVerticalGroup;
            root.DefaultDockable = mainVerticalGroup;
            root.VisibleDockables = _dockFactory.CreateList<IDockable>(mainVerticalGroup);

            _dockFactory.ContextLocator = new Dictionary<string, Func<object>>
            {
                [cueListModule.Id] = () => cueListVm,
                [sceneryViewerModule.Id] = () => sceneryViewerVm,
                [controllerViewerModule.Id] = () => controllerViewerVm,
                [loggerModule.Id] = () => loggerVm
            };

            _dockFactory.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IHostWindow)] = () => new HostWindow()
            };

            root.Factory = _dockFactory;
            _dockFactory.InitLayout(root);

            return root;
        }

        public void SpawnNewModuleTab<T>(string? customTitle = null) where T : ModuleViewModelBase
        {
            if (_dockFactory.FindDockable(RootLayout, d => d.Id == "MainDocuments") is not IDocumentDock documentDock)
                return;

            var newVm = _serviceProvider.GetRequiredService<T>();
            newVm.Title = customTitle ?? newVm.Title;
            var newModule = _dockFactory.CreateDocument();
            newModule.Title = newVm.Title;
            newModule.Context = newVm;
            newModule.Id = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(customTitle))
            {
                newModule.Title = customTitle;
            }
            newModule.Context = newVm;
            _dockFactory.ContextLocator ??= new Dictionary<string, Func<object>>();
            _dockFactory.ContextLocator[newModule.Id] = () => newVm;

            _dockFactory.AddDockable(documentDock, newModule);
            
            _dockFactory.SetActiveDockable(newModule);
        }

        public void ShowAxisViewer() => SpawnNewModuleTab<AxisViewerModuleViewModel>("Axis Viewer");
        public void ShowSceneryViewer() => SpawnNewModuleTab<SceneryViewerModuleViewModel>("Scenery Viewer");
        public void ShowCueList() => SpawnNewModuleTab<CueListModuleViewModel>("Cue List");
        public void ShowControllerViewer() => SpawnNewModuleTab<ControllerViewerModuleViewModel>("Controllers");
        public void ShowCueStack() => SpawnNewModuleTab<CueStackModuleViewModel>("Cue Stack");
        public void ShowLogger() => SpawnNewModuleTab<LoggerModuleViewModel>("System Log");
    }
}
