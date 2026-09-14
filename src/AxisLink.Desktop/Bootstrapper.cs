using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.Utilities;
using AxisLink.Desktop.ViewModels;
using AxisLink.Desktop.ViewModels.Modules;
using AxisLink.Desktop.ViewModels.Windows;
using AxisLink.Desktop.ViewModels.Windows.AxisSetup;
using AxisLink.Desktop.ViewModels.Windows.ControllerCommunication;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using AxisLink.Desktop.ViewModels.Windows.CueCreation;
using AxisLink.Desktop.ViewModels.Windows.GroupsWindow;
using AxisLink.Desktop.ViewModels.Windows.JogWindow;
using AxisLink.Desktop.ViewModels.Windows.PatchWindow;
using AxisLink.Desktop.ViewModels.Windows.ScenerySetup;
using AxisLink.Desktop.ViewModels.Windows.SensorsWindow;
using AxisLink.Desktop.ViewModels.Dev;
using AxisLink.Infrastructure.Factories;
using AxisLink.Infrastructure.Loggers;
using AxisLink.Infrastructure.ShowFileStorage;
using AxisLink.Infrastructure.Sprockets;
using Dock.Model.Core;
using Microsoft.Extensions.DependencyInjection;
using System;




namespace AxisLink.Desktop
{
    public static class Bootstrapper
    {
        public static IServiceProvider CreateServiceProvider()
        {
            // Create a new service collection
            var services = new ServiceCollection();

            // Create singletons
            services.AddSingleton<IShowFileStorage, XmlShowFileStorage>();
            services.AddSingleton<IMotionServiceFactory, MotionServiceFactory>();
            services.AddSingleton<IConsoleLogger, ConsoleLogger>();
            // UI Services
            services.AddSingleton<FileDialogService>();
            services.AddSingleton<WindowManager>();
            services.AddSingleton<WorkspaceManager>();
            services.AddSingleton<IFactory, DockFactory>();
            services.AddSingleton<ISprocketProvider, SprocketProvider>();
            // Set up managers
            services.AddSingleton<MotionManager>();
            services.AddSingleton<CueManager>();
            services.AddSingleton<ShowFileManager>();
            services.AddSingleton<UnitManager>();
            services.AddSingleton<NetworkManager>();
            

            // Add UI VMs as Transients (new instance created each time requested)
            // Main Windows
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LaunchWindowViewModel>();
            
            // Modules
            services.AddTransient<LoggerModuleViewModel>();
            services.AddTransient<CueListModuleViewModel>();
            services.AddTransient<CueStackModuleViewModel>();
            services.AddTransient<AxisViewerModuleViewModel>();
            services.AddTransient<SceneryViewerModuleViewModel>();
            services.AddTransient<ControllerViewerModuleViewModel>();

            // Windows
            services.AddTransient<ScenerySetupViewModel>();
            services.AddTransient<AxisSetupViewModel>();
            services.AddTransient<ControllerCommunicationViewModel>();
            services.AddTransient<ControllerSetupViewModel>();
            services.AddTransient<CueCreationWindowViewModel>();
            services.AddTransient<GroupsWindowViewModel>();
            services.AddTransient<JogWindowViewModel>();
            services.AddTransient<PatchWindowViewModel>();
            services.AddTransient<ProjectPreferencesViewModel>();
            services.AddTransient<SensorWindowViewModel>();

            // Dev
            services.AddTransient<SimulatedAxisDevWindowViewModel>();

            return services.BuildServiceProvider();
            
        }
    }
}
