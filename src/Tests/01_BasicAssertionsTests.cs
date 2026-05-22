using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 01 — Basic Assertions
///
/// The most fundamental xUnit assertions:
///   - Assert.Equal       : checks two values are equal
///   - Assert.NotEqual    : checks two values are different
///   - Assert.True/False  : checks a boolean condition
///   - Assert.Same        : checks reference equality
/// Every test method is decorated with [Fact], which tells xUnit to run it.
/// </summary>
public class BasicAssertionsTests
{
    private readonly Calculator _calc = new();

    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        // Arrange
        int a = 3, b = 5;

        // Act
        int result = _calc.Add(a, b);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Subtract_LargerFromSmaller_ReturnsNegative()
    {
        int result = _calc.Subtract(3, 10);
        Assert.Equal(-7, result);
    }

    [Fact]
    public void Multiply_ByZero_ReturnsZero()
    {
        int result = _calc.Multiply(99, 0);
        Assert.Equal(0, result);
    }

    [Fact]
    public void Multiply_NegativeNumbers_ReturnsPositive()
    {
        int result = _calc.Multiply(-4, -3);
        Assert.True(result > 0, "Negative × negative should be positive.");
        Assert.Equal(12, result);
    }

    [Fact]
    public void Add_Zero_ReturnsSameValue()
    {
        int result = _calc.Add(42, 0);
        Assert.NotEqual(0, result);
        Assert.Equal(42, result);
    }

    [Fact]
    public void TwoCalculators_AreNotSameReference()
    {
        var calc1 = new Calculator();
        var calc2 = new Calculator();
        Assert.NotSame(calc1, calc2); // different object references
    }

    [Fact]
    public void TwoCalculators_SameReference_AreSame()
    {
        var calc1 = new Calculator();
        var calc2 = calc1;             // same reference
        Assert.Same(calc1, calc2);
    }
}
