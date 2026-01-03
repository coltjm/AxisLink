using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinetiCUE.Services;

namespace KinetiCUE.Modules.Core.ViewModels
{
    internal partial class ProjectSetupViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _projectName = "New Project";
        [ObservableProperty]
        private string _authorName = "Programmer";
        [ObservableProperty]
        private string _distanceUnits = "in";

        public ProjectSetupViewModel()
        {
        }

        [RelayCommand]
        private void Save(Window window)
        {
            window.DialogResult = true;
            FileManager.Instance.CurrentShow.ShowName = ProjectName;
            FileManager.Instance.CurrentShow.Author = AuthorName;
            FileManager.Instance.CurrentShow.DistanceUnits = DistanceUnits;
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
