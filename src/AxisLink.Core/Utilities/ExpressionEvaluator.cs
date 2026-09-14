using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace AxisLink.Core.Utilities;

public static class ExpressionEvaluator
{
    private static readonly DataTable Table = new();

    public static double Evaluate(string expression, IReadOnlyDictionary<string, double>? variables = null)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return 0;

        string sanitized = expression.Trim();

        if (variables != null)
        {
            foreach (var (varName, value) in variables)
            {
                sanitized = sanitized.Replace(
                    varName,
                    value.ToString(CultureInfo.InvariantCulture),
                    StringComparison.OrdinalIgnoreCase);
            }
        }
        try
        {
            lock (Table) // DataTable is not thread-safe for concurrent Compute calls
            {
                object result = Table.Compute(sanitized, string.Empty);
                return Convert.ToDouble(result, CultureInfo.InvariantCulture);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to evaluate expression: '{sanitized}'", ex);
        }
    }

    public static int EvaluateInt(string expression, IReadOnlyDictionary<string, double>? variables = null)
    {
        return (int)Math.Round(Evaluate(expression, variables));
    }
}