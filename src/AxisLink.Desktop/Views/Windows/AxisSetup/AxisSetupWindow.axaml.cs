using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisLink.Desktop.ViewModels.Windows.AxisSetup;
using System;

namespace AxisLink.Desktop.Views.Windows.AxisSetup;

public partial class AxisSetupWindow : Window
{
    public AxisSetupWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is AxisSetupViewModel vm)
        {
            // Link the ViewModel's action to the Window's Close method
            vm.RequestClose = () => this.Close();
        }
    }
}