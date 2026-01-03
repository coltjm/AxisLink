using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KinetiCUE.Modules.Core.ViewModels
{
    internal partial class SavePromptViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _versionLabel = "autosave" + DateTime.Now.ToString("_yyyyMMdd_HHmmss");

        [ObservableProperty]
        private bool _isMilestone = false;
        public bool IsCanceled = true;

        public SavePromptViewModel()
        {

        }

        [RelayCommand]
        private void Save(Window window)
        {
            window.DialogResult = true;
            window.Close();

        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }
}
