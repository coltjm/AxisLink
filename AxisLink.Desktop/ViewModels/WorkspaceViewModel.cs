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
using AxisLink.Core.Management;

namespace AxisLink.Desktop.ViewModels
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
            private readonly IConsoleLogger _logger;
            private readonly ShowFileManager FileManager;
        public ObservableCollection<ModuleViewModelBase> ActiveModules { get; } = new();

            public WorkspaceViewModel(IConsoleLogger logger, ShowFileManager fileManager)
            {
                _logger = logger;
                FileManager = fileManager;

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
                ActiveModules.Add(new AxisViewerModuleViewModel(FileManager));
            }

            public void ShowCueList()
            {
                ActiveModules.Add(new CueListModuleViewModel());
            }
    }
    }
