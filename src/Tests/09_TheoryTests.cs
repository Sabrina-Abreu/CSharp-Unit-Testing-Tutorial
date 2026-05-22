using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 09 — Parameterized Tests with [Theory]
///
/// [Theory] lets you run the same test with multiple data sets:
///   - [InlineData]   : values inline in the attribute
///   - [MemberData]   : values from a static property or method
///   - [ClassData]    : values from a class implementing IEnumerable
/// This eliminates copy-paste and ensures thorough coverage.
/// </summary>
public class TheoryTests
{
    private readonly MathHelper _math = new();

    // --- InlineData examples ---

    [Theory]
    [InlineData(2,  true)]
    [InlineData(3,  true)]
    [InlineData(5,  true)]
    [InlineData(13, true)]
    [InlineData(4,  false)]
    [InlineData(1,  false)]
    [InlineData(0,  false)]
    [InlineData(-7, false)]
    public void IsPrime_VariousInputs(int n, bool expected)
    {
        Assert.Equal(expected, _math.IsPrime(n));
    }

    [Theory]
    [InlineData(0,   32)]
    [InlineData(100, 212)]
    [InlineData(-40, -40)]   // -40°C == -40°F
    public void CelsiusToFahrenheit_KnownValues(double celsius, double expectedFahrenheit)
    {
        double result = _math.CelsiusToFahrenheit(celsius);
        Assert.Equal(expectedFahrenheit, result, precision: 5);
    }

    [Theory]
    [InlineData(0,  1)]
    [InlineData(1,  1)]
    [InlineData(5,  120)]
    [InlineData(10, 3628800)]
    public void Factorial_InlineData(int n, int expected)
    {
        Assert.Equal(expected, _math.Factorial(n));
    }

    // --- MemberData example ---

    public static IEnumerable<object[]> GcdTestCases =>
    [
        [12,  8,  4],
        [15,  5,  5],
        [7,  13,  1],
        [100, 75, 25],
    ];

    [Theory]
    [MemberData(nameof(GcdTestCases))]
    public void Gcd_MemberData(int a, int b, int expected)
    {
        Assert.Equal(expected, _math.Gcd(a, b));
    }

    // --- ClassData example ---

    public class EvenOddData : TheoryData<int, bool>
    {
        public EvenOddData()
        {
            Add(0,  true);
            Add(2,  true);
            Add(-4, true);
            Add(1,  false);
            Add(99, false);
        }
    }

    [Theory]
    [ClassData(typeof(EvenOddData))]
    public void IsEven_ClassData(int n, bool expected)
    {
        Assert.Equal(expected, _math.IsEven(n));
    }
}
