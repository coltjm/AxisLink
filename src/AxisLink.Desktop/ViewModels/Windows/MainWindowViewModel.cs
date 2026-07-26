using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Controls;
using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.Utilities;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using AxisLink.Infrastructure.Loggers; // Lightweight, high-performance standard
using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;
using AxisLink.Desktop.Views.Windows.CueCreation;
using AxisLink.Desktop.ViewModels.Windows.CueCreation;
using AxisLink.Desktop.Views.Windows.ProjectPreferences;
namespace AxisLink.Desktop.ViewModels.Windows
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public WorkspaceManager Workspace { get; }

        private readonly ShowFileManager FileManager;
        private readonly WindowManager windowManager;
        private readonly FileDialogService fileDialogService;
        private readonly IConsoleLogger Logger;

        // Dynamic properties for menu checking states
        private bool _isLogConsoleVisible = true;
        public bool IsLogConsoleVisible
        {
            get => _isLogConsoleVisible;
        }

        // DI Constructor
        public MainWindowViewModel(WorkspaceManager workspace, IConsoleLogger logger, ShowFileManager fileManager, WindowManager _windowManager, FileDialogService _fileDialogService)
        {
            Workspace = workspace;
            Logger = logger;
            FileManager = fileManager;
            windowManager = _windowManager;
            fileDialogService = _fileDialogService;
        }

        // Parameterless constructor for the Avalonia Previewer
        public MainWindowViewModel()
        {
            Workspace = new WorkspaceManager(new ConsoleLogger(null), null, null);
            Logger = new ConsoleLogger(null);
        }

        // --- BACKEND LOGIC ACTIONS ---

        [RelayCommand]
        private void NewShow()
        {
            // Prompt save first

            FileManager.NewShow();
            // Reopen main window to reset the workspace and UI state?
            //windowManager.ShowMainWindow();
            Logger.LogInfo("File -> New Show triggered.");
        }

        [RelayCommand]  
        private async Task OpenShow()
        {
            string? selectedPath = await fileDialogService.OpenFileDialogAsync(
                title: "Open AxisLink Project",
                extensions: new[] { "*.xml", "*.alink", "*.txt" },
                filterName: "Project Files"
            );

            if (!string.IsNullOrEmpty(selectedPath))
            {
                Logger.LogInfo("Opening Show: " + selectedPath);
                FileManager.OpenShow(selectedPath);
                windowManager.ShowMainWindow();
                Logger.LogInfo("Opened show file successfully: " + FileManager.CurrentShow?.Header?.ShowName);

            }
            Logger.LogInfo("File -> Open Show triggered.");
            Logger.LogInfo(FileManager.CurrentShow.Machinery.Axes.ToString());
        }
        [RelayCommand]
        private void SaveShow()
        {
            Logger.LogInfo("File -> Save Show triggered.");
        }
        [RelayCommand]
        private async Task SaveAsShow()
        {
            string[]? selectedPath = await fileDialogService.SaveFileDialogAsync(
                title: "Save AxisLink Project",
                showName: FileManager.CurrentShow?.Header?.ShowName ?? "New Show"
            );

            if (!string.IsNullOrEmpty(selectedPath[0]) && selectedPath[1] == "AxisLink Project")
            {
                Logger.LogInfo("Saving Show: " + selectedPath[0]);
                FileManager.SaveShow(selectedPath[0]);
                Logger.LogInfo("Saved show file successfully: " + FileManager.CurrentShow?.Header?.ShowName);

            }
            Logger.LogInfo("File -> Save As triggered.");
        }
        [RelayCommand]
        private void ExitApplication()
        {
            Logger.LogInfo("Application shutting down via File -> Exit.");

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }

        [RelayCommand]
        private void OpenNewControllerWindow()
        {
            windowManager.ShowControllerSetupWindow();
        }

        [RelayCommand]
        private void OpenNewAxisWindow()
        {
            windowManager.ShowAxisSetupWindow();
        }

        [RelayCommand]
        private void OpenNewCueWindow()
        {
            windowManager.ShowWindow<CueCreationWindowViewModel, CueCreationWindow>();
        }

        [RelayCommand]
        private void OpenNewProjectPreferencesWindow()
        {
            windowManager.ShowWindow<ProjectPreferencesViewModel, ProjectPreferencesWindow>();
        }

        [RelayCommand]
        private void ShowAxisViewer()
        {
            Logger.LogInfo("Showing Axis Viewer.");
            Workspace.ShowAxisViewer();
        }
        [RelayCommand]
        private void ShowCueSheet()
        {
            Logger.LogInfo("Showing Cue Sheet.");
            Workspace.ShowCueList();
        }

        [RelayCommand]
        private void ShowControllerViewer()
        {
            Logger.LogInfo("Showing Controller Viewer.");
            Workspace.ShowControllerViewer();
        }

        [RelayCommand]
        private void ShowLogConsole()
        {
            Logger.LogInfo("Showing Log Console.");
            Workspace.ShowLogger();
        }

        [RelayCommand]
        private void ShowCueStack()
        {
            Logger.LogInfo("Showing Cue Stack");
            Workspace.ShowCueStack();
        }
    }
}