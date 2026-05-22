using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 05 — Exception Tests
///
/// Testing that code throws the right exceptions is essential.
///   - Assert.Throws&lt;T&gt;   : verifies the specific exception type is thrown
///   - Assert.ThrowsAsync&lt;T&gt; : for async methods
///   - Inspecting exception properties (Message, Amount, etc.)
/// </summary>
public class ExceptionTests
{
    [Fact]
    public void Withdraw_MoreThanBalance_ThrowsInsufficientFundsException()
    {
        var account = new BankAccount("Alice", 100m);

        var ex = Assert.Throws<InsufficientFundsException>(() => account.Withdraw(200m));

        Assert.Equal(200m, ex.Amount);
        Assert.Contains("200", ex.Message);
    }

    [Fact]
    public void Withdraw_AfterException_BalanceUnchanged()
    {
        var account = new BankAccount("Bob", 50m);

        Assert.Throws<InsufficientFundsException>(() => account.Withdraw(100m));

        // Balance must NOT have changed after the failed withdrawal
        Assert.Equal(50m, account.Balance);
    }

    [Fact]
    public void Deposit_NegativeAmount_ThrowsArgumentOutOfRangeException()
    {
        var account = new BankAccount("Carol", 100m);

        Assert.Throws<ArgumentOutOfRangeException>(() => account.Deposit(-10m));
    }

    [Fact]
    public void BankAccount_EmptyOwnerName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new BankAccount(""));
        Assert.Throws<ArgumentException>(() => new BankAccount("   "));
    }

    [Fact]
    public void BankAccount_NegativeInitialBalance_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BankAccount("Dave", -100m));
    }

    [Fact]
    public void Calculator_DivideByZero_ThrowsDivideByZeroException()
    {
        var calc = new Calculator();
        var ex = Assert.Throws<DivideByZeroException>(() => calc.Divide(10, 0));
        Assert.NotEmpty(ex.Message);
    }

    [Fact]
    public void MathHelper_Factorial_NegativeInput_ThrowsArgumentOutOfRange()
    {
        var math = new MathHelper();
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => math.Factorial(-1));
        Assert.Equal("n", ex.ParamName);
    }

    [Fact]
    public void Calculator_CircleArea_NegativeRadius_ThrowsArgumentOutOfRange()
    {
        var calc = new Calculator();
        Assert.Throws<ArgumentOutOfRangeException>(() => calc.CircleArea(-1));
    }
}
