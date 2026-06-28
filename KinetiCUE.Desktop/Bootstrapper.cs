using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using KinetiCUE.Core.Interfaces;
using KinetiCUE.Infrastructure.Services;
using KinetiCUE.ViewModels;
using KinetiCUE.Core.Management;


namespace KinetiCUE.Desktop
{
    public static class Bootstrapper
    {
        public static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            
            services.AddSingleton<IFileStorage, ShowFileService>();

            // Set up managers
            services.AddSingleton<MotionManager>();
            services.AddSingleton<CueManager>();
            services.AddSingleton<ShowFileManager>();

            // Add UI VMs as Transients
            services.AddTransient<MainWindowViewModel>();

            return services.BuildServiceProvider();
            
        }
    }
}
