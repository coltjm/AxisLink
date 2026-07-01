using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KinetiCUE.Desktop.ViewModels.Windows.ControllerSetup;

namespace KinetiCUE.Desktop.Views.Windows.ControllerSetup;

public partial class ControllerSetupWindow : Window
{
    public ControllerSetupWindow()
    {
        InitializeComponent();
    }

    // Overloaded constructor that takes the data context directly
    public ControllerSetupWindow(ControllerSetupViewModel viewModel) : this()
    {
        DataContext = viewModel;

        // Wire up the callback action so the ViewModel can close this window view
        viewModel.RequestClose = () => this.Close();
    }
}