using FluentAssertions;
using Library;
using Moq;
using Xunit;

namespace Tests;

/// <summary>
/// Example 19 — Fluent Assertions
///
/// FluentAssertions replaces Assert.X() with a readable English-like syntax:
///   result.Should().Be(expected)
///   result.Should().NotBeNull()
///   collection.Should().HaveCount(3).And.Contain(x => x.Name == "Alice")
///
/// Benefits: failure messages include full context and the API is
/// discoverable through IntelliSense.
/// </summary>
public class FluentAssertionsTests
{
    private readonly Calculator  _calc   = new();
    private readonly MathHelper  _math   = new();
    private readonly StringHelper _str   = new();

    // --- Numeric ---

    [Fact]
    public void Add_TwoNumbers_FluentEqual()
    {
        _calc.Add(3, 4).Should().Be(7);
    }

    [Fact]
    public void CircleArea_UnitCircle_ShouldBeApproximatelyPi()
    {
        _calc.CircleArea(1.0).Should().BeApproximately(Math.PI, precision: 1e-10);
    }

    [Fact]
    public void Divide_ResultShouldBePositive()
    {
        _calc.Divide(10, 3).Should().BeGreaterThan(0).And.BeApproximately(3.333, 0.001);
    }

    // --- Boolean ---

    [Fact]
    public void IsPrime_Five_ShouldBeTrue()
    {
        _math.IsPrime(5).Should().BeTrue();
    }

    [Fact]
    public void IsPrime_Four_ShouldBeFalse()
    {
        _math.IsPrime(4).Should().BeFalse();
    }

    // --- String ---

    [Fact]
    public void Reverse_Hello_ShouldBeOlleh()
    {
        _str.Reverse("hello")
            .Should().Be("olleh")
            .And.HaveLength(5)
            .And.StartWith("o");
    }

    [Fact]
    public void MaskEmail_ShouldContainAtSign()
    {
        _str.MaskEmail("john@example.com")
            .Should().Contain("@")
            .And.StartWith("j")
            .And.EndWith(".com");
    }

    // --- Collections ---

    [Fact]
    public void FibonacciSequence_ShouldContainExpectedValues()
    {
        var fib = _math.FibonacciSequence(6).ToList();
        fib.Should().HaveCount(6)
           .And.StartWith(0)
           .And.Contain(5);
    }

    [Fact]
    public void ShoppingCart_Items_ShouldAllHavePositivePrice()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("A", 5m, 1));
        cart.Add(new CartItem("B", 3m, 2));

        cart.Items.Should().NotBeEmpty()
            .And.HaveCount(2)
            .And.OnlyContain(i => i.Price > 0);
    }

    // --- Exception ---

    [Fact]
    public void Divide_ByZero_ShouldThrowDivideByZeroException()
    {
        Action act = () => _calc.Divide(1, 0);
        act.Should().Throw<DivideByZeroException>()
           .WithMessage("*zero*");
    }

    // --- Null ---

    [Fact]
    public void EmptyCart_MostExpensive_ShouldBeNull()
    {
        var cart = new ShoppingCart();
        cart.MostExpensive().Should().BeNull();
    }

    [Fact]
    public void NonEmptyCart_MostExpensive_ShouldNotBeNull()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Gold", 999m, 1));
        cart.MostExpensive().Should().NotBeNull()
            .And.Match<CartItem>(i => i.Name == "Gold");
    }
}
