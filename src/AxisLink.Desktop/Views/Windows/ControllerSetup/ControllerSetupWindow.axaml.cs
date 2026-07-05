using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisLink.Desktop.ViewModels.Windows.AxisSetup;
using AxisLink.Desktop.ViewModels.Windows.ControllerSetup;
using System;

namespace AxisLink.Desktop.Views.Windows.ControllerSetup;

public partial class ControllerSetupWindow : Window
{
    public ControllerSetupWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is ControllerSetupViewModel vm)
        {
            // Link the ViewModel's action to the Window's Close method
            vm.RequestClose = () => this.Close();
        }
    }
}