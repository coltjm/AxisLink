using KinetiCUE.Core.Interfaces;
using KinetiCUE.Core.Management;
using KinetiCUE.Desktop.ViewModels;
using KinetiCUE.Desktop.ViewModels.Modules;
using KinetiCUE.Desktop.ViewModels.Windows;
using KinetiCUE.Infrastructure.Factories;
using KinetiCUE.Infrastructure.Loggers;
using KinetiCUE.Infrastructure.ShowFileStorge;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;




namespace KinetiCUE.Desktop
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
            services.AddSingleton<IKQLogger, KQLogger>();
            // Set up managers
            services.AddSingleton<MotionManager>();
            services.AddSingleton<CueManager>();
            services.AddSingleton<ShowFileManager>();

            // Add UI VMs as Transients (new instance created each time requested)
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<WorkspaceViewModel>();
            services.AddTransient<LoggerModuleViewModel>();
            services.AddTransient<CueListModuleViewModel>();
            services.AddTransient<AxisViewerModuleViewModel>();
            services.AddTransient<ControllerSetupViewModel>();

            return services.BuildServiceProvider();
            
        }
    }
}
