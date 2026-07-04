using Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using AxisLink.Core.Interfaces;
using AxisLink.Desktop.Utilities;
using AxisLink.Desktop.ViewModels.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AxisLink.Desktop.ViewModels
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
            private readonly IKQLogger _logger;

            public ObservableCollection<ModuleViewModelBase> ActiveModules { get; } = new();

            public WorkspaceViewModel(IKQLogger logger)
            {
                _logger = logger;

                InitializeDefaultWorkspace();
            }

            private void InitializeDefaultWorkspace()
            {
                
                var loggerModule = new LoggerModuleViewModel(_logger) { Title = "System Log Console" };
                ActiveModules.Add(loggerModule);

            }

            public void ShowAxisViewer()
            {
                // This architecture allows you to spin up multiple copies effortlessly
                ActiveModules.Add(new AxisViewerModuleViewModel());
            }

            public void ShowCueList()
            {
                ActiveModules.Add(new CueListModuleViewModel());
            }
    }
    }
