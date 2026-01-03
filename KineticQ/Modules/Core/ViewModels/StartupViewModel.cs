using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinetiCUE.Services;
using Microsoft.Win32;

namespace KinetiCUE.Modules.Core.ViewModels
{
    public partial class StartupViewModel : ObservableObject
    {
        // Action to tell the View to close (true = Launch Main Window)
        public Action<bool>? RequestClose { get; set; }

        [RelayCommand]
        private void NewProject()
        {
            // 1. Create a fresh show with defaults ("New Show", "Inches")
            FileManager.Instance.NewShow();

            // 2. Launch the Main Window immediately
            RequestClose?.Invoke(true);
        }

        [RelayCommand]
        private void OpenProject()
        {
            var dialog = new OpenFileDialog { Filter = "KinetiCUE Project (*.kcue)|*.kcue" };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    FileManager.Instance.LoadShow(dialog.FileName);
                    RequestClose?.Invoke(true);
                }
                catch
                {
                    // (Optional) Show error message
                }
            }
        }
    }
}