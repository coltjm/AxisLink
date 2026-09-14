using CommunityToolkit.Mvvm.ComponentModel;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public partial class SceneryAxisItemViewModel : ObservableObject
    {
        [ObservableProperty] private int _axisId;
        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private float _currentPosition;
        [ObservableProperty] private float _currentVelocity;
        [ObservableProperty] private float _lowLimit = 0;
        [ObservableProperty] private float _highLimit = 1000;
        [ObservableProperty] private bool _isEnabled;
        [ObservableProperty] private bool _isFaulted;
    }
}