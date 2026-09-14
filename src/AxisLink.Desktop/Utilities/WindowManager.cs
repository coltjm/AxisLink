using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using AxisLink.Desktop.ViewModels;
using AxisLink.Desktop.ViewModels.Windows;
using AxisLink.Desktop.ViewModels.Windows.AxisSetup;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using AxisLink.Desktop.Views.Windows;
using AxisLink.Desktop.Views.Windows.AxisSetup;
using AxisLink.Desktop.ViewModels.Windows.JogWindow;
using AxisLink.Desktop.Views.Windows.ControllerSetup;
using AxisLink.Desktop.Views.Windows.JogWindow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

        public void ShowJogWindow(int sceneryId)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var fileManager = serviceProvider.GetRequiredService<ShowFileManager>();
                var motionManager = serviceProvider.GetRequiredService<MotionManager>();

                var vm = new JogWindowViewModel(sceneryId, fileManager, motionManager);
                var window = new JogWindow { DataContext = vm };

                window.Show(desktop.MainWindow);
            }
        }

    }
}
