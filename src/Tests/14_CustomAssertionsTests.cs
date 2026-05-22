using Library;
using Xunit;

namespace Tests;

// --------------------------------------------------------------------------
// Custom assertion helpers
// --------------------------------------------------------------------------

/// <summary>
/// Encapsulates multi-step assertions about a ShoppingCart into readable,
/// reusable helper methods. This avoids duplicating Assert calls across tests.
/// </summary>
internal static class CartAssert
{
    public static void HasItemCount(ShoppingCart cart, int expected) =>
        Assert.Equal(expected, cart.Count);

    public static void TotalEquals(ShoppingCart cart, decimal expected) =>
        Assert.Equal(expected, cart.Total);

    public static void ContainsItemNamed(ShoppingCart cart, string name) =>
        Assert.Contains(cart.Items, i => i.Name == name);

    public static void DoesNotContainItemNamed(ShoppingCart cart, string name) =>
        Assert.DoesNotContain(cart.Items, i => i.Name == name);

    public static void AllItemsHavePositivePrice(ShoppingCart cart) =>
        Assert.All(cart.Items, i => Assert.True(i.Price > 0, $"Item '{i.Name}' has non-positive price."));
}

internal static class StringAssertExtensions
{
    public static void IsPascalCase(string value)
    {
        Assert.False(string.IsNullOrEmpty(value), "String should not be empty.");
        Assert.True(char.IsUpper(value[0]), $"'{value}' should start with an uppercase letter.");
        Assert.DoesNotContain(" ", value); // no spaces in PascalCase
    }
}

// --------------------------------------------------------------------------
// Tests that use the custom assertion helpers
// --------------------------------------------------------------------------

/// <summary>
/// Example 14 — Custom Assertions
///
/// When the same multi-step assertion pattern repeats across tests,
/// extract it into a named helper. This makes tests:
///   - More readable ("what" not "how")
///   - Easier to maintain (fix one place)
///   - Self-documenting
/// </summary>
public class CustomAssertionsTests
{
    [Fact]
    public void Cart_AfterAddingThreeItems_HasCorrectCountAndTotal()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Apple",  1.00m, 2)); // 2.00
        cart.Add(new CartItem("Bread",  2.50m, 1)); // 2.50
        cart.Add(new CartItem("Butter", 3.00m, 1)); // 3.00

        CartAssert.HasItemCount(cart, 3);
        CartAssert.TotalEquals(cart, 7.50m);
        CartAssert.AllItemsHavePositivePrice(cart);
        CartAssert.ContainsItemNamed(cart, "Bread");
    }

    [Fact]
    public void Cart_AfterRemovingItem_DoesNotContainIt()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Apple", 1.00m, 1));
        cart.Add(new CartItem("Milk",  0.80m, 2));
        cart.Remove("Apple");

        CartAssert.DoesNotContainItemNamed(cart, "Apple");
        CartAssert.ContainsItemNamed(cart, "Milk");
        CartAssert.HasItemCount(cart, 1);
    }

    [Fact]
    public void ToPascalCase_ProducesValidPascalCaseString()
    {
        var helper = new StringHelper();
        string result = helper.ToPascalCase("hello beautiful world");

        StringAssertExtensions.IsPascalCase(result);
        Assert.Equal("HelloBeautifulWorld", result);
    }

    [Fact]
    public void AllItemsHavePositivePrice_PassesForWellFormedCart()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("X", 10m, 3));
        cart.Add(new CartItem("Y", 0.01m, 1));

        CartAssert.AllItemsHavePositivePrice(cart); // should not throw
    }
}
