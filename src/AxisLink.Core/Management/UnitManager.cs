using AxisLink.Core.Models.Configs;
using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AxisLink.Core.Management
{
    public class UnitManager
    {
        private readonly ShowFileManager _showFileManager;

        public UnitManager(ShowFileManager showFileManager)
        {
            _showFileManager = showFileManager;
        }

        private ProjectConfig Config => _showFileManager.CurrentShow.ProjectConfig;

        public LinearUnitEnum LinearUnit => Config.LinearUnit;
        public RotationalUnitEnum RotationalUnit => Config.RotationalUnit;

        public string LinearUnitSymbol => Config.LinearUnit switch
        {
            LinearUnitEnum.Millimeter => "mm",
            LinearUnitEnum.Centimeter => "cm",
            LinearUnitEnum.Meter => "m",
            LinearUnitEnum.Inch => "in",
            LinearUnitEnum.Foot => "ft",
            _ => "units"
        };

        public string RotationalUnitSymbol => Config.RotationalUnit switch
        {
            RotationalUnitEnum.Degree => "°",
            RotationalUnitEnum.Radian => "rad",
            RotationalUnitEnum.Revolution => "rev",
            _ => "units"
        };



        // Display Conversions
        public double ConvertLinearToDisplay(double valInMm) => valInMm / GetMmScale(Config.LinearUnit);
        public double ConvertLinearToStorage(double valInDisplay) => valInDisplay * GetMmScale(Config.LinearUnit);

        public double ConvertRotationalToDisplay(double valInDeg) => valInDeg / GetDegScale(Config.RotationalUnit);
        public double ConvertRotationalToStorage(double valInDisplay) => valInDisplay * GetDegScale(Config.RotationalUnit);

        // Smart Parsing: Accepts 3', 3ft, 36in, 36", 1.5m, 1500mm
        public float ParseLinearToMm(string input, float fallbackMm)
        {
            if (string.IsNullOrWhiteSpace(input)) return fallbackMm;

            try
            {
                string cleanInput = input.Trim().ToLowerInvariant();

                // 1. Expand foot-inch shorthand: 5'6" -> (5ft + 6in)
                cleanInput = Regex.Replace(cleanInput, @"(\d+)'\s*(\d+)""", "($1ft + $2in)");

                string expressionInMm = Regex.Replace(cleanInput, @"(\d*\.?\d+)\s*([a-z""']*)", match =>
                {
                    if (!double.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        return match.Value;

                    string unit = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(unit))
                    {
                        double scale = unit switch
                        {
                            "'" or "ft" or "feet" => 304.8,
                            "\"" or "in" or "inch" or "inches" => 25.4,
                            "mm" => 1.0,
                            "cm" => 10.0,
                            "m" => 1000.0,
                            _ => GetMmScale(Config.LinearUnit)
                        };
                        return (val * scale).ToString(CultureInfo.InvariantCulture);
                    }

                    int matchIndex = match.Index;
                    int matchLength = match.Length;
                    string priorText = cleanInput.Substring(0, matchIndex).TrimEnd();
                    string trailingText = cleanInput.Substring(matchIndex + matchLength).TrimStart();

                    bool isScalar = priorText.EndsWith("*") || priorText.EndsWith("/") ||
                        trailingText.StartsWith("*") || trailingText.StartsWith("/");

                    if (isScalar)
                    {
                        return val.ToString(CultureInfo.InvariantCulture); 
                    }

                    return (val * GetMmScale(Config.LinearUnit)).ToString(CultureInfo.InvariantCulture);
                });

                var table = new DataTable();
                var resultInMm = Convert.ToDouble(table.Compute(expressionInMm, string.Empty));

                return (float)resultInMm;
            }
            catch
            {
                return fallbackMm;
            }
        }

        public float ParseRotationalToDeg(string input, float fallbackDeg)
        {
            if (string.IsNullOrWhiteSpace(input)) return fallbackDeg;

            try
            {
                string cleanInput = input.Trim().ToLowerInvariant();

                string expressionInDeg = Regex.Replace(cleanInput, @"(\d*\.?\d+)\s*([a-z°]*)", match =>
                {
                    if (!double.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        return match.Value;

                    string unit = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(unit))
                    {
                        double scale = unit switch
                        {
                            "°" or "deg" or "degree" or "degrees" => 1.0,
                            "rad" or "radian" or "radians" => 57.29577951308232,
                            "rev" or "rot" or "revolution" or "revolutions" => 360.0,
                            _ => GetDegScale(Config.RotationalUnit)
                        };
                        return (val * scale).ToString(CultureInfo.InvariantCulture);
                    }

                    int matchIndex = match.Index;
                    int matchLength = match.Length;
                    string priorText = cleanInput.Substring(0, matchIndex).TrimEnd();
                    string trailingText = cleanInput.Substring(matchIndex + matchLength).TrimStart();

                    bool isScalar = priorText.EndsWith("*") || priorText.EndsWith("/") ||
                        trailingText.StartsWith("*") || trailingText.StartsWith("/");
                    if (isScalar)
                    {
                        return val.ToString(CultureInfo.InvariantCulture); 
                    }

                    return (val * GetDegScale(Config.RotationalUnit)).ToString(CultureInfo.InvariantCulture);
                });

                var table = new DataTable();
                var resultInDeg = Convert.ToDouble(table.Compute(expressionInDeg, string.Empty));

                return (float)resultInDeg;
            }
            catch
            {
                return fallbackDeg;
            }
        }

        private double GetMmScale(LinearUnitEnum unit) => unit switch
            {
                LinearUnitEnum.Millimeter => 1.0,
                LinearUnitEnum.Centimeter => 10.0,
                LinearUnitEnum.Meter => 1000.0,
                LinearUnitEnum.Inch => 25.4,
                LinearUnitEnum.Foot => 304.8,
                LinearUnitEnum.Custom => Config.CustomLinearUnit ?? 1.0,
                _ => 1.0
            };

        private double GetDegScale(RotationalUnitEnum unit) => unit switch
        {
            RotationalUnitEnum.Degree => 1.0,
            RotationalUnitEnum.Radian => 57.29577951308232,
            RotationalUnitEnum.Revolution => 360.0,
            RotationalUnitEnum.Custom => Config.CustomRotationationalUnit ?? 1.0,
            _ => 1.0
        };
    }
}