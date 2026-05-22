using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 13 — Floating-Point Tests
///
/// Floating-point arithmetic is inexact. Never use == to compare doubles.
///   - Assert.Equal(expected, actual, precision) : rounds to N decimal places
///   - Assert.Equal(expected, actual, tolerance) : absolute tolerance
///   - Assert.InRange for bounded checks
/// </summary>
public class FloatingPointTests
{
    private readonly Calculator _calc = new();
    private readonly MathHelper _math = new();

    [Fact]
    public void Divide_TenByThree_HasCorrectPrecision()
    {
        double result = _calc.Divide(10, 3);
        // Compare with 4 decimal-place precision → 3.3333
        Assert.Equal(3.3333, result, precision: 4);
    }

    [Fact]
    public void CircleArea_RadiusOne_ApproximatelyPi()
    {
        double result = _calc.CircleArea(1.0);
        // π ≈ 3.14159265…; precision: 5 means round to 5 decimal places
        Assert.Equal(Math.PI, result, precision: 5);
    }

    [Fact]
    public void CircleArea_RadiusTwo_IsApproximatelyFourPi()
    {
        double result = _calc.CircleArea(2.0);
        Assert.Equal(4 * Math.PI, result, precision: 10);
    }

    [Fact]
    public void SquareRoot_OfTwo_PrecisionCheck()
    {
        double result = _calc.SquareRoot(2.0);
        Assert.Equal(1.41421, result, precision: 5);
    }

    [Fact]
    public void CelsiusToFahrenheit_BodyTemp_IsWithinRange()
    {
        double fahrenheit = _math.CelsiusToFahrenheit(37.0); // normal body temp
        // 98.6°F ± 0.01
        Assert.InRange(fahrenheit, 98.59, 98.61);
    }

    [Fact]
    public void RoundTrip_CelsiusToFahrenheitAndBack_ReturnsSameValue()
    {
        double original   = 23.7;
        double fahrenheit = _math.CelsiusToFahrenheit(original);
        double backToCelsius = _math.FahrenheitToCelsius(fahrenheit);

        // Allow tiny floating-point error
        Assert.Equal(original, backToCelsius, precision: 10);
    }

    [Fact]
    public void Power_TwoToThePowerTen_IsExact()
    {
        // 2^10 is always exactly 1024, representable in IEEE 754
        double result = _calc.Power(2, 10);
        Assert.Equal(1024.0, result);          // exact — no precision needed
    }

    [Fact]
    public void CircleArea_NegativeRadius_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _calc.CircleArea(-5));
    }
}
