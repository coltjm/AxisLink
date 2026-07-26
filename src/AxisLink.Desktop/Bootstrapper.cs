using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.Utilities;
using AxisLink.Desktop.ViewModels;
using AxisLink.Desktop.ViewModels.Modules;
using AxisLink.Desktop.ViewModels.Windows;
using AxisLink.Desktop.ViewModels.Windows.AxisSetup;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using AxisLink.Desktop.ViewModels.Windows.CueCreation;
using AxisLink.Infrastructure.Factories;
using AxisLink.Infrastructure.Loggers;
using AxisLink.Infrastructure.ShowFileStorage;
using Dock.Model.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;




namespace AxisLink.Desktop
{
    public static class Bootstrapper
    {
        public static IServiceProvider CreateServiceProvider()
        {
            // Create a new service collection
            var services = new ServiceCollection();

            // Create file storage singleton
            services.AddSingleton<IShowFileStorage, XmlShowFileStorage>();
            services.AddSingleton<IMotionServiceFactory, MotionServiceFactory>();
            services.AddSingleton<IConsoleLogger, ConsoleLogger>();
            // UI Services
            services.AddSingleton<FileDialogService>();
            services.AddSingleton<WindowManager>();
            services.AddSingleton<WorkspaceManager>();
            services.AddSingleton<IFactory, DockFactory>();
            // Set up managers
            services.AddSingleton<MotionManager>();
            services.AddSingleton<CueManager>();
            services.AddSingleton<ShowFileManager>();
            services.AddSingleton<UnitManager>();
            

            // Add UI VMs as Transients (new instance created each time requested)
            // Main Windows
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LaunchWindowViewModel>();
            
            // Modules
            services.AddTransient<LoggerModuleViewModel>();
            services.AddTransient<CueListModuleViewModel>();
            services.AddTransient<CueStackModuleViewModel>();
            services.AddTransient<AxisViewerModuleViewModel>();
            services.AddTransient<ControllerViewerModuleViewModel>();

            // Windows
            services.AddTransient<ControllerSetupViewModel>();
            services.AddTransient<AxisSetupViewModel>();
            services.AddTransient<CueCreationWindowViewModel>();
            services.AddTransient<ProjectPreferencesViewModel>();
            


            return services.BuildServiceProvider();
            
        }
    }
}
