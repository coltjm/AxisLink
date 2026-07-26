using Avalonia.Controls;
using AxisLink.Desktop.ViewModels;

namespace AxisLink.Desktop.Views.Windows.ProjectPreferences;

public partial class ProjectPreferencesWindow : Window
{
    public ProjectPreferencesWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is ProjectPreferencesViewModel vm)
        {
            vm.RequestClose += result => Close(result);
        }
    }
}