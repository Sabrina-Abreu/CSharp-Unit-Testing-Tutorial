using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 03 — Number / Math Tests
///
/// Covers arithmetic edge cases:
///   - Negative numbers, zero, large values
///   - Assert.Equal for integers
///   - Assert.InRange to check a value is within bounds
///   - Assert.True / False for computed conditions
/// </summary>
public class NumberTests
{
    private readonly MathHelper _math = new();
    private readonly Calculator _calc = new();

    [Fact]
    public void Factorial_Zero_ReturnsOne()
    {
        Assert.Equal(1, _math.Factorial(0));
    }

    [Fact]
    public void Factorial_Five_Returns120()
    {
        Assert.Equal(120, _math.Factorial(5));
    }

    [Fact]
    public void Gcd_TwelveAndEight_ReturnsFour()
    {
        Assert.Equal(4, _math.Gcd(12, 8));
    }

    [Fact]
    public void Gcd_PrimeNumbers_ReturnsOne()
    {
        Assert.Equal(1, _math.Gcd(7, 13));
    }

    [Fact]
    public void IsEven_EvenNumber_ReturnsTrue()
    {
        Assert.True(_math.IsEven(4));
        Assert.True(_math.IsEven(0));
        Assert.True(_math.IsEven(-2));
    }

    [Fact]
    public void IsEven_OddNumber_ReturnsFalse()
    {
        Assert.False(_math.IsEven(7));
    }

    [Fact]
    public void IsInRange_ValueInsideBounds_ReturnsTrue()
    {
        Assert.True(_math.IsInRange(50, 1, 100));
    }

    [Fact]
    public void IsInRange_ValueOutsideBounds_ReturnsFalse()
    {
        Assert.False(_math.IsInRange(101, 1, 100));
    }

    [Fact]
    public void FibonacciSequence_FirstSeven_MatchesExpected()
    {
        var fib = _math.FibonacciSequence(7).ToList();
        Assert.Equal([0, 1, 1, 2, 3, 5, 8], fib);
    }

    [Fact]
    public void Calculator_Power_ReturnsCorrectValue()
    {
        double result = _calc.Power(2, 10);
        Assert.Equal(1024, result);
    }

    [Fact]
    public void Calculator_Add_LargeNumbers_DoesNotOverflow()
    {
        // Using long arithmetic to verify
        int a = int.MaxValue / 2;
        int b = int.MaxValue / 2;
        int result = _calc.Add(a, b);
        Assert.InRange(result, 0, int.MaxValue);
    }
}
