using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using KinetiCUE.Core.Interfaces;
using KinetiCUE.Core.Management;
using KinetiCUE.Core.Models.Configs;
using KinetiCUE.Desktop;
using KinetiCUE.Desktop.Utilities;
using KinetiCUE.Desktop.ViewModels;
using KinetiCUE.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KinetiCUE.Desktop
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
            var logger = serviceProvider.GetRequiredService<IKQLogger>();

            try
            {
                // TODO Read theme from config file or user settings, if available. If not, use default theme.
                var defaultOrSavedTheme = new KQThemeConfig();

                // Inject the colors into Avalonia's runtime resource dictionary repository
                DynamicThemeEngine.ApplyCustomTheme(defaultOrSavedTheme);
                logger.LogInfo("Custom theme settings loaded successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to load custom theme settings: {ex.Message}");
            }

            // TEMP TESTING
            ConsoleTesting.Test(serviceProvider);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainViewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}