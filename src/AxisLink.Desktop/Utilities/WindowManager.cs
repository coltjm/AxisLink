using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AxisLink.Desktop.Views.Windows;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AxisLink.Desktop.ViewModels.Windows;
using AxisLink.Desktop.Views.Windows.ControllerSetup;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using AxisLink.Desktop.Views.Windows.AxisSetup;
using AxisLink.Desktop.ViewModels.Windows.AxisSetup;

namespace AxisLink.Desktop.Utilities
{
    public class WindowManager
    {
        private readonly IServiceProvider serviceProvider;
        public WindowManager(IServiceProvider ServiceProvider)
        {
            serviceProvider = ServiceProvider;
        }
        public void ShowLaunchWindow()
        {
            if(Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            { 
                var launchWindow = new LaunchWindow
                {
                    DataContext = serviceProvider.GetRequiredService<LaunchWindowViewModel>()
                };
                var oldWindow = desktop.MainWindow;
                desktop.MainWindow = launchWindow;
                launchWindow.Show();
                oldWindow?.Close();
                
            }
            
        }
        public void ShowMainWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
                };
                var oldWindow = desktop.MainWindow;
                desktop.MainWindow = mainWindow;
                mainWindow.Show();
                oldWindow?.Close();
                
            }
        }

        public void ShowControllerSetupWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var controllerSetupWindow = new ControllerSetupWindow
                {
                    DataContext = serviceProvider.GetRequiredService<ControllerSetupViewModel>()
                };
                controllerSetupWindow.ShowDialog(desktop.MainWindow);
            }
        }

        public void ShowAxisSetupWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var axisSetupWindow = new AxisSetupWindow
                {
                    DataContext = serviceProvider.GetRequiredService<AxisSetupViewModel>()
                };
                axisSetupWindow.ShowDialog(desktop.MainWindow);
            }
        }

    }
}
