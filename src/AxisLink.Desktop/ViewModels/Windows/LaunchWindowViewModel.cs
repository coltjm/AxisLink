using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.Utilities;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AxisLink.Desktop.ViewModels.Windows
{
    public partial class LaunchWindowViewModel : ViewModelBase
    {
        private readonly ShowFileManager FileManager;
        public IConsoleLogger Logger { get; }
        private readonly FileDialogService fileDialogService;
        private readonly IServiceProvider ServiceProvider;
        private readonly WindowManager windowManager;

        public LaunchWindowViewModel(IConsoleLogger logger, ShowFileManager fileManager, FileDialogService _fileDialogService, IServiceProvider serviceProvider, WindowManager _windowManager)
        {
            Logger = logger;
            FileManager = fileManager;
            fileDialogService = _fileDialogService;
            ServiceProvider = serviceProvider;
            windowManager = _windowManager;

        }
        [RelayCommand]
        private void NewShow()
        {
            // Create a new show file and initialize the workspace
            FileManager.NewShow();
            windowManager.ShowMainWindow();

            Logger.LogInfo("File -> New Show triggered.");
        }
        [RelayCommand]
        private async Task OpenShow()
        {
            // Open a file dialog to select an existing show file
            string? selectedPath = await fileDialogService.OpenFileDialogAsync(
                title: "Open AxisLink Project",
                extensions: new[] { "*.xml", "*.alink" , "*.txt"},
                filterName: "Project Files"
            );
            // If a file was selected, open it and initialize the workspace
            if (!string.IsNullOrEmpty(selectedPath))
            {
                Logger.LogInfo("Opening Show: " + selectedPath);
                FileManager.OpenShow(selectedPath);
                windowManager.ShowMainWindow();
                Logger.LogInfo("Opened show file successfully: "+ FileManager.CurrentShow?.Header?.ShowName);
                
            }
        }
        [RelayCommand]
        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
