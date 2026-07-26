using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AxisLink.Core.Management;
using System.Diagnostics;
using Avalonia.Threading;

namespace AxisLink.Desktop.Controls
{
    public partial class UnitTextBox : UserControl
    {
        public enum UnitType
        {
            Linear,
            LinearSpeed,
            LinearAccel,
            Rotational,
            RotationalSpeed,
            RotationalAccel,
            Percentage
        }

        public static readonly StyledProperty<UnitType> ModeProperty =
            AvaloniaProperty.Register<UnitTextBox, UnitType>(nameof(Mode), UnitType.Linear);

        public static readonly StyledProperty<float?> ValueProperty =
            AvaloniaProperty.Register<UnitTextBox, float?>(nameof(Value), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public static readonly StyledProperty<string> UnitSuffixProperty =
            AvaloniaProperty.Register<UnitTextBox, string>(nameof(UnitSuffix), string.Empty);

        private static UnitManager? _unitManager;
        public static UnitManager? UnitManager
        {
            get => _unitManager;
            set
            {
                _unitManager = value;
            }
        }

        public UnitType Mode
        {
            get => GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public float? Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public string UnitSuffix
        {
            get => GetValue(UnitSuffixProperty);
            set => SetValue(UnitSuffixProperty, value);
        }

        public UnitTextBox()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            FormatDisplay();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ModeProperty)
            {
                FormatDisplay();
            }
            else if (change.Property == ValueProperty && !InputBox.IsFocused)
            {
                FormatDisplay();
            }
        }

        private string GetCurrentSuffix()
        {
            if (UnitManager == null) return string.Empty;

            return Mode switch
            {
                UnitType.Linear => UnitManager.LinearUnitSymbol,
                UnitType.LinearSpeed => $"{UnitManager.LinearUnitSymbol}/s",
                UnitType.LinearAccel => $"{UnitManager.LinearUnitSymbol}/s²",
                UnitType.Rotational => UnitManager.RotationalUnitSymbol,
                UnitType.RotationalSpeed => $"{UnitManager.RotationalUnitSymbol}/s",
                UnitType.RotationalAccel => $"{UnitManager.RotationalUnitSymbol}/s²",
                UnitType.Percentage => "%",
                _ => string.Empty
            };
        }

        private void CommitInput()
        {
            Debug.WriteLine("commit input");
            if (UnitManager == null) return;

            float current = Value ?? 0f;

            float parsedValue = Mode switch
            {
                UnitType.Linear or UnitType.LinearSpeed or UnitType.LinearAccel
                    => UnitManager.ParseLinearToMm(InputBox.Text ?? string.Empty, current),

                UnitType.Rotational or UnitType.RotationalSpeed or UnitType.RotationalAccel
                    => UnitManager.ParseRotationalToDeg(InputBox.Text ?? string.Empty, current),

                UnitType.Percentage => float.TryParse(InputBox.Text?.Replace("%", "").Trim(), out float p) ? p : current,

                _ => current
            };

            Value = parsedValue;
            FormatDisplay();
        }

        

        private void FormatDisplay()
        {
            if (!Value.HasValue)
            {
                InputBox.Text = string.Empty;
                return;
            }

            double displayVal = Mode switch
            {
                UnitType.Linear or UnitType.LinearSpeed or UnitType.LinearAccel
                    => UnitManager?.ConvertLinearToDisplay(Value.Value) ?? Value.Value,

                UnitType.Rotational or UnitType.RotationalSpeed or UnitType.RotationalAccel
                    => UnitManager?.ConvertRotationalToDisplay(Value.Value) ?? Value.Value,

                UnitType.Percentage => Value.Value,

                _ => Value.Value
            };

            string suffix = GetCurrentSuffix();

            InputBox.Text = string.IsNullOrEmpty(suffix)
                ? displayVal.ToString("F2")
                : $"{displayVal:F2} {suffix}";
        }


        private void OnInputGotFocus(object? sender, FocusChangedEventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                InputBox.SelectAll();
            });
        }

        private void OnInputLostFocus(object? sender, FocusChangedEventArgs e) => CommitInput();

        private void OnInputKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CommitInput();
                e.Handled = true;
            }
        }
    }
}