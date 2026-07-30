using Avalonia.Data.Converters;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AxisLink.Desktop.Utilities
{
    public class StringToKeyGestureConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string gestureString && !string.IsNullOrWhiteSpace(gestureString))
            {
                return KeyGesture.Parse(gestureString);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
