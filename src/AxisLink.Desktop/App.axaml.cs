using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Configs;
using AxisLink.Desktop;
using AxisLink.Desktop.Utilities;
using AxisLink.Desktop.ViewModels.Windows;
using AxisLink.Desktop.Views;
using AxisLink.Desktop.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AxisLink.Desktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            IServiceProvider serviceProvider = Bootstrapper.CreateServiceProvider();
            var windowManager = serviceProvider.GetRequiredService<WindowManager>();
            var logger = serviceProvider.GetRequiredService<IConsoleLogger>();

            try
            {
                // TODO Read theme from config file or user settings, if available. If not, use default theme.
                var defaultOrSavedTheme = new AppThemeConfig();

                // Inject the colors into Avalonia's runtime resource dictionary repository
                DynamicThemeEngine.ApplyCustomTheme(defaultOrSavedTheme);
                logger.LogInfo("Custom theme settings loaded successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to load custom theme settings: {ex.Message}");
            }

            // TEMP TESTING
            //ConsoleTesting.Test(serviceProvider);

            windowManager.ShowLaunchWindow();

            base.OnFrameworkInitializationCompleted();
        }
    }
}