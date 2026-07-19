using Avalonia.Data.Converters;
using Avalonia.Media;
using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Logging;
using System;
using System.Globalization;

namespace AxisLink.Desktop.Utilities
{
    // Converts LogSeverity to a corresponding color for UI representation
    public class LogSeverityToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is LogLevel severity)
            {
                return severity switch
                {
                    LogLevel.Warning => Avalonia.Application.Current?.Resources["ConsoleWarningText"] ?? Brushes.Orange,
                    LogLevel.Error => Avalonia.Application.Current?.Resources["ConsoleErrorText"] ?? Brushes.Red,
                    LogLevel.Critical => Avalonia.Application.Current?.Resources["ConsoleCriticalText"] ?? Brushes.DarkRed,
                    _ => Avalonia.Application.Current?.Resources["ConsoleText"] ?? Brushes.LightGreen
                };
            }
            return Brushes.LightGreen;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}