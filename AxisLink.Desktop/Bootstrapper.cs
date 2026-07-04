using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.ViewModels;
using AxisLink.Desktop.ViewModels.Modules;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using AxisLink.Infrastructure.Factories;
using AxisLink.Infrastructure.Loggers;
using AxisLink.Infrastructure.ShowFileStorge;
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
