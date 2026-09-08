using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisLink.Desktop.ViewModels.Windows.ScenerySetup;
using System;

namespace AxisLink.Desktop.Views.Windows.ScenerySetup;

public partial class ScenerySetupWindow : Window
{
    public ScenerySetupWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is ScenerySetupViewModel vm)
        {
            vm.RequestClose = () => this.Close();
        }
    }
}