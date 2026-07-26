using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AxisLink.Core.Models.Configs;
using Dock.Model.Core;
using AxisLink.Core.Management;

namespace AxisLink.Desktop.ViewModels;

public partial class ProjectPreferencesViewModel : ViewModelBase
{
    private readonly ProjectConfig _targetConfig;

    [ObservableProperty]
    private float _cueSpacing;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCustomLinear))]
    private LinearUnitEnum _linearUnit;

    [ObservableProperty]
    private float? _customLinearUnit;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCustomRotational))]
    private RotationalUnitEnum _rotationalUnit;

    [ObservableProperty]
    private float? _customRotationalUnit;

    [ObservableProperty]
    private bool _enableStacks;

    [ObservableProperty]
    private int _loggerDisplayCap;

    [ObservableProperty]
    private bool _mathInInput;

    public bool IsCustomLinear => LinearUnit == LinearUnitEnum.Custom;
    public bool IsCustomRotational => RotationalUnit == RotationalUnitEnum.Custom;

    public LinearUnitEnum[] LinearUnits => Enum.GetValues<LinearUnitEnum>();
    public RotationalUnitEnum[] RotationalUnits => Enum.GetValues<RotationalUnitEnum>();

    // Action invoked when the window needs to close
    public event Action<bool>? RequestClose;

    public ProjectPreferencesViewModel(ShowFileManager showFileManager)
    {
        ProjectConfig activeConfig = showFileManager.CurrentShow.ProjectConfig;
        _targetConfig = activeConfig;

        // Populate working values
        CueSpacing = activeConfig.CueSpacing;
        LinearUnit = activeConfig.LinearUnit;
        CustomLinearUnit = activeConfig.CustomLinearUnit;
        RotationalUnit = activeConfig.RotationalUnit;
        CustomRotationalUnit = activeConfig.CustomRotationationalUnit;
        EnableStacks = activeConfig.EnableStacks;
        LoggerDisplayCap = activeConfig.LoggerDisplayCap;
        MathInInput = activeConfig.MathInInput;
    }

    [RelayCommand]
    private void Save()
    {
        // Apply back to the active project config
        _targetConfig.CueSpacing = CueSpacing;
        _targetConfig.LinearUnit = LinearUnit;
        _targetConfig.CustomLinearUnit = CustomLinearUnit;
        _targetConfig.RotationalUnit = RotationalUnit;
        _targetConfig.CustomRotationationalUnit = CustomRotationalUnit;
        _targetConfig.EnableStacks = EnableStacks;
        _targetConfig.LoggerDisplayCap = LoggerDisplayCap;
        _targetConfig.MathInInput = MathInInput;

        // Signal window close with success
        RequestClose?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}