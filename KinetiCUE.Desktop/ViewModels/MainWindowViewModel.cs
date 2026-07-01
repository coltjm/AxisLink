using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Controls;
using KinetiCUE.Core.Interfaces;
using KinetiCUE.Core.Management;
using KinetiCUE.Desktop.Utilities;
using KinetiCUE.Desktop.ViewModels.Windows.ControllerSetup;
using KinetiCUE.Infrastructure.Loggers; // Lightweight, high-performance standard
using System;
using System.Diagnostics;
using System.Windows.Input;
namespace KinetiCUE.Desktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public WorkspaceViewModel Workspace { get; }

        public ShowFileManager FileManager { get; }
        public IRootDock Layout { get; set;}
        public IKQLogger Logger { get;}

        // --- STANDARD MVVM COMMANDS ---
        // ICommand is the native .NET interface that Avalonia's XAML understands out of the box.
        public ICommand NewShowCommand { get; }
        public ICommand OpenShowCommand { get; }
        public ICommand SaveShowCommand { get; }
        public ICommand SaveAsShowCommand { get; }
        public ICommand ExitApplicationCommand { get; }
        public ICommand ShowAxisViewerCommand { get; }
        public ICommand ShowCueSheetCommand { get; }
        public ICommand OpenNewControllerWindowCommand { get; }

        // Dynamic properties for menu checking states
        private bool _isLogConsoleVisible = true;
        public bool IsLogConsoleVisible
        {
            get => _isLogConsoleVisible;
        }

        // DI Constructor
        public MainWindowViewModel(WorkspaceViewModel workspace, IKQLogger logger, ShowFileManager fileManager)
        {
            Workspace = workspace;
            Logger = logger;
            FileManager = fileManager;

            NewShowCommand = new RelayCommand(ExecuteNewShow);
            OpenShowCommand = new RelayCommand(ExecuteOpenShow);
            SaveShowCommand = new RelayCommand(ExecuteSaveShow);
            SaveAsShowCommand = new RelayCommand(ExecuteSaveAsShow);
            ExitApplicationCommand = new RelayCommand(ExecuteExitApplication);
            OpenNewControllerWindowCommand = new RelayCommand(OpenNewControllerWindow);
            ShowAxisViewerCommand = new RelayCommand(() => Workspace.ShowAxisViewer());
            ShowCueSheetCommand = new RelayCommand(() => Workspace.ShowCueList());
        }

        // Parameterless constructor for the Avalonia Previewer
        public MainWindowViewModel()
        {
            Workspace = new WorkspaceViewModel(new KQLogger());
            Logger = new KQLogger();
            NewShowCommand = new RelayCommand(() => { });
            OpenShowCommand = new RelayCommand(() => { });
            SaveShowCommand = new RelayCommand(() => { });
            SaveAsShowCommand = new RelayCommand(() => { });
            ExitApplicationCommand = new RelayCommand(() => { });
        }

        // --- BACKEND LOGIC ACTIONS ---

        private void ExecuteNewShow()
        {
            Logger.LogInfo("File -> New Show triggered.");
        }

        private void ExecuteOpenShow()
        {
            Logger.LogInfo("File -> Open Show triggered.");
        }

        private void ExecuteSaveShow()
        {
            Logger.LogInfo("File -> Save Show triggered.");
        }

        private void ExecuteSaveAsShow()
        {
            Logger.LogInfo("File -> Save As triggered.");
        }

        private void ExecuteExitApplication()
        {
            Logger.LogInfo("Application shutting down via File -> Exit.");

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }

        private void OpenNewControllerWindow()
        {
            Logger.LogInfo("Opening new Controller Setup window.");
            var configVm = new ControllerSetupViewModel(FileManager);

            var setupWindow = new Views.Windows.ControllerSetup.ControllerSetupWindow(configVm);

            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                setupWindow.ShowDialog(desktop.MainWindow);
            }
            else
            {
                Logger.LogError("Failed to open Controller Setup window: Application lifetime is not IClassicDesktopStyleApplicationLifetime.");
            }
        }
    }
}