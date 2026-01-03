
using KineticQ.Services;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using KineticQ.Modules.Core.Views;

namespace KineticQ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Direct File Launch (Double click file)
            if (e.Args.Length > 0 && System.IO.File.Exists(e.Args[0]))
            {
                FileManager.Instance.LoadShow(e.Args[0]);
                LaunchMainWindow();
                return;
            }

            // 2. Normal Launch -> Show Startup Screen
            ShowStartupScreen();
        }

        private void ShowStartupScreen()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var vm = new KineticQ.Modules.Core.ViewModels.StartupViewModel();
            var window = new KineticQ.Modules.Core.Views.StartupWindow(); 
            window.DataContext = vm;

            // Connect the VM's close request to the Window
            vm.RequestClose = (shouldLaunchMain) =>
            {
                window.DialogResult = shouldLaunchMain;
                window.Close();
            };

            // Wait for the user to finish
            if (window.ShowDialog() == true)
            {
                LaunchMainWindow();
            }
            else
            {
                Shutdown();
            }
        }

        private void LaunchMainWindow()
        {
            var main = new MainWindow(); 
            Application.Current.MainWindow = main;
            main.Show();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }



}
