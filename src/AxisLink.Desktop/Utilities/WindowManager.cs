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
using Avalonia.Controls;
using AxisLink.Desktop.ViewModels;

namespace AxisLink.Desktop.Utilities
{
    public class WindowManager
    {
        private readonly IServiceProvider serviceProvider;
        public WindowManager(IServiceProvider ServiceProvider)
        {
            serviceProvider = ServiceProvider;
        }

        // Open the launch window and close the previos main window
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

        // Open the main window and close the launch window
        public void ShowMainWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var oldWindow = desktop.MainWindow;

                var mainWindow = new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
                };

                desktop.MainWindow = mainWindow;
                mainWindow.Show();

                oldWindow?.Close();
            }
        }

        public void ShowWindow<VM, View>() 
            where VM : ViewModelBase 
            where View : Window, new()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var window = new View
                {
                    DataContext = serviceProvider.GetRequiredService<VM>()
                };
                window.Show();
            }
        }

        // Open the controller setup window as a dialog, with the main window as the owner
        public void ShowControllerSetupWindow() => ShowWindow<ControllerSetupViewModel, ControllerSetupWindow>();

        // Open the axis setup window as a dialog, with the main window as the owner
        public void ShowAxisSetupWindow() => ShowWindow<AxisSetupViewModel, AxisSetupWindow>();

        

    }
}
