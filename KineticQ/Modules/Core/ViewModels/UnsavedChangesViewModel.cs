using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KinetiCUE.Modules.Core.ViewModels
{
    internal partial class UnsavedChangesViewModel : ObservableObject
    {

        public UnsavedChangesViewModel()
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
