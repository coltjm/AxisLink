using AxisLink.Core.Management;
using AxisLink.Core.Models.Configs;
using FluentAssertions;
using Xunit;

namespace AxisLink.Tests.Management;

public class UnitManagerTests
{
    [Theory]
    [InlineData("2ft*2+300mm", 1519.2f)]
    [InlineData("1m - 500mm", 500.0f)]
    [InlineData("3ft / 2", 457.2f)]

    // Decimals and fractional inputs
    [InlineData("0.5m + 1.5in", 538.1f)]       // 500mm + 38.1mm
    [InlineData("10.5in * 2", 533.4f)]         // 266.7mm * 2

    // Spaces inside expression
    [InlineData("2 ft * 2 + 300 mm", 1519.2f)]
    [InlineData(" 100mm + 1ft ", 404.8f)]

    // Operator precedence (multiplication/division before addition/subtraction)
    [InlineData("100mm + 2in * 2", 201.6f)]    // 100mm + (50.8mm * 2) = 201.6mm
    [InlineData("1m - 10in / 2", 873.0f)]      // 1000mm - (254mm / 2) = 873.0mm

    // Parentheses handling
    [InlineData("(100mm + 2in) * 2", 301.6f)]  // (100mm + 50.8mm) * 2 = 301.6mm
    [InlineData("(1ft - 6in) / 2", 76.2f)]     // (304.8mm - 152.4mm) / 2 = 76.2mm

    // Negative positions / directional offsets
    [InlineData("-1ft + 100mm", -204.8f)]      // -304.8mm + 100mm
    [InlineData("-25.4mm * 2", -50.8f)]
    public void ParseLinearInput_WithMixedUits_ReturnsCorrectMillimeters(string input, float expectedMm)
    {
        var config = new ProjectConfig
        {
            LinearUnit = LinearUnitEnum.Inch
        };
        var fileManager = new ShowFileManager();

        fileManager.NewShow();
        fileManager.CurrentShow.ProjectConfig = config;

        var unitManager = new UnitManager(fileManager);

        float resultInMm = unitManager.ParseLinearToMm(input, 0f);

        resultInMm.Should().BeApproximately(expectedMm, precision: 0.1f);
    }

    [Theory]
    [InlineData("1in", 25.4f)]
    [InlineData("1ft", 304.8f)]
    [InlineData("100mm", 100.0f)]
    [InlineData("1m", 1000.0f)]
    public void ConvertLinearToMm_StandardUnits_ReturnsExpectedValue(string input, float expectedMm)
    {

        var unitManager = new UnitManager(new ShowFileManager());

        float result = unitManager.ParseLinearToMm(input, 0f);

        result.Should().BeApproximately(expectedMm, precision: 0.1f);
    }

    [Theory]
    [InlineData("1in", 1.0f)]
    [InlineData("1ft", 12.0f)]
    [InlineData("100mm", 3.93701f)]
    [InlineData("1m", 39.3701f)]
    public void ConvertLinearToDisplay_StandardUnits_ReturnsExpectedValue(string input, float expectedIn)
    {
        var config = new ProjectConfig
        {
            LinearUnit = LinearUnitEnum.Inch
        };
        var fileManager = new ShowFileManager();

        fileManager.NewShow();
        fileManager.CurrentShow.ProjectConfig = config;

        var unitManager = new UnitManager(fileManager);

        double result = unitManager.ConvertLinearToDisplay(unitManager.ParseLinearToMm(input, 0f));

        result.Should().BeApproximately(expectedIn, precision: 0.1f);
    }

    [Theory]
    // Malformed input should safely return the fallback value (e.g., 0f or current position)
    [InlineData("2ft * * 2", 0.0f)]
    [InlineData("abc", 0.0f)]
    [InlineData("100mm + ", 0.0f)]
    public void ParseLinearToMm_InvalidExpression_ReturnsFallback(string input, float expectedFallback)
    {
        var unitManager = new UnitManager(new ShowFileManager());

        float result = unitManager.ParseLinearToMm(input, fallbackMm: expectedFallback);

        result.Should().Be(expectedFallback);
    }

    [Theory]
    // Additive terms default to project unit (Inches = 25.4mm)
    [InlineData("2 + 3mm", 53.8f)]         // 50.8mm + 3mm
    [InlineData("100mm + 2", 150.8f)]      // 100mm + 50.8mm
    [InlineData("2 - 10mm", 40.8f)]        // 50.8mm - 10mm

    // Multipliers/Divisors act as raw scalars
    [InlineData("2 * 3mm", 6.0f)]          // 2 * 3mm
    [InlineData("1m / 2", 500.0f)]         // 1000mm / 2
    [InlineData("2in * 3", 152.4f)]        // 50.8mm * 3

    // Mixed precedence & parenthesis
    [InlineData("2 + 3mm * 2", 56.8f)]     // 50.8mm + (3mm * 2) = 56.8mm
    [InlineData("(2 + 3mm) * 2", 107.6f)]   // (50.8mm + 3mm) * 2 = 107.6mm
    public void ParseLinearToMm_OperatorContext_EvaluatesCorrectly(string input, float expectedMm)
    {
        var config = new ProjectConfig { LinearUnit = LinearUnitEnum.Inch };
        var fileManager = new ShowFileManager();
        fileManager.NewShow();
        fileManager.CurrentShow.ProjectConfig = config;

        var unitManager = new UnitManager(fileManager);

        float result = unitManager.ParseLinearToMm(input, 0f);

        result.Should().BeApproximately(expectedMm, precision: 0.1f);
    }
}
