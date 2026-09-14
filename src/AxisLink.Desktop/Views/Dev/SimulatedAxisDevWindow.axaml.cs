using Avalonia.Controls;
using AxisLink.Desktop.ViewModels.Dev;


namespace AxisLink.Desktop.Views.Dev
{
    public partial class SimulatedAxisDevWindow : Window
    {
        public SimulatedAxisDevWindow()
        {
            InitializeComponent();
            Closing += (s, e) =>
            {
                if (DataContext is SimulatedAxisDevWindowViewModel vm)
                {
                    vm.Dispose();
                }
            };
        }
    }
}