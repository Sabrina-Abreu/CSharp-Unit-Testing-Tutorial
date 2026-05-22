using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 06 — Boolean / Guard Tests
///
/// Tests for business rules expressed as boolean conditions:
///   - Assert.True / Assert.False
///   - Testing guard clauses that validate input
///   - Boundary value analysis (just inside/outside limits)
/// </summary>
public class BooleanTests
{
    private readonly PasswordValidator _pwdValidator = new();
    private readonly AgeValidator      _ageValidator = new();
    private readonly EmailValidator    _emailValidator = new();

    // --- Password ---
    [Fact]
    public void Password_ValidComplex_ReturnsTrue()
    {
        Assert.True(_pwdValidator.IsValid("Secret@123"));
    }

    [Fact]
    public void Password_TooShort_ReturnsFalse()
    {
        Assert.False(_pwdValidator.IsValid("Ab1!"));
    }

    [Fact]
    public void Password_NoSpecialChar_ReturnsFalse()
    {
        Assert.False(_pwdValidator.IsValid("Secret123"));
    }

    [Fact]
    public void Password_Null_ReturnsFalse()
    {
        Assert.False(_pwdValidator.IsValid(null));
    }

    [Fact]
    public void Password_StrongPassword_ReturnsStrongStrength()
    {
        string strength = _pwdValidator.GetStrength("SuperSecret@99!!!");
        Assert.Equal("Strong", strength);
    }

    [Fact]
    public void Password_WeakPassword_ReturnsWeakStrength()
    {
        string strength = _pwdValidator.GetStrength("abc");
        Assert.Equal("Weak", strength);
    }

    // --- Age ---
    [Fact]
    public void Age_Exactly18_IsAdult()
    {
        Assert.True(_ageValidator.IsAdult(18));   // boundary: inclusive
    }

    [Fact]
    public void Age_17_IsNotAdult()
    {
        Assert.False(_ageValidator.IsAdult(17));  // boundary: one below
    }

    [Fact]
    public void Age_Zero_IsValidAge()
    {
        Assert.True(_ageValidator.IsValidAge(0));
    }

    [Fact]
    public void Age_Negative_IsInvalidAge()
    {
        Assert.False(_ageValidator.IsValidAge(-1));
    }

    [Fact]
    public void Age_GetCategory_Teenager()
    {
        Assert.Equal("Teenager", _ageValidator.GetCategory(16));
    }

    // --- Email ---
    [Fact]
    public void Email_Valid_ReturnsTrue()
    {
        Assert.True(_emailValidator.IsValid("user@example.com"));
    }

    [Fact]
    public void Email_MissingAt_ReturnsFalse()
    {
        Assert.False(_emailValidator.IsValid("userexample.com"));
    }

    [Fact]
    public void Email_Empty_ReturnsFalse()
    {
        Assert.False(_emailValidator.IsValid(""));
    }
}
