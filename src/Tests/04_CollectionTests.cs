using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 04 — Collection Assertions
///
/// xUnit collection helpers:
///   - Assert.Empty / Assert.NotEmpty
///   - Assert.Contains / Assert.DoesNotContain (with predicate)
///   - Assert.Equal on collections (order-sensitive)
///   - Assert.All — checks every element satisfies a condition
///   - Assert.Single — exactly one element
/// </summary>
public class CollectionTests
{
    [Fact]
    public void NewCart_IsEmpty()
    {
        var cart = new ShoppingCart();
        Assert.Empty(cart.Items);
        Assert.True(cart.IsEmpty);
    }

    [Fact]
    public void AddItem_CartHasOneItem()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Apple", 1.50m, 3));
        Assert.Single(cart.Items);
        Assert.NotEmpty(cart.Items);
    }

    [Fact]
    public void AddMultipleItems_CountIsCorrect()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Apple", 1.50m, 1));
        cart.Add(new CartItem("Bread", 2.00m, 2));
        cart.Add(new CartItem("Milk",  0.99m, 1));
        Assert.Equal(3, cart.Count);
    }

    [Fact]
    public void Items_AllHavePositivePrice()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("A", 5m, 1));
        cart.Add(new CartItem("B", 3m, 2));
        cart.Add(new CartItem("C", 1m, 4));

        Assert.All(cart.Items, item => Assert.True(item.Price > 0));
    }

    [Fact]
    public void RemoveItem_ItemIsGone()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Apple", 1.50m, 1));
        cart.Add(new CartItem("Bread", 2.00m, 1));
        cart.Remove("Apple");

        Assert.DoesNotContain(cart.Items, i => i.Name == "Apple");
        Assert.Contains(cart.Items, i => i.Name == "Bread");
    }

    [Fact]
    public void Total_SumsAllItemPricesTimesQuantity()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Apple", 1.00m, 3)); // 3.00
        cart.Add(new CartItem("Bread", 2.50m, 2)); // 5.00
        Assert.Equal(8.00m, cart.Total);
    }

    [Fact]
    public void Clear_RemovesAllItems()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("X", 1m, 1));
        cart.Clear();
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void MostExpensive_ReturnsHighestPricedItem()
    {
        var cart = new ShoppingCart();
        cart.Add(new CartItem("Cheap",     1m, 1));
        cart.Add(new CartItem("Expensive", 50m, 1));
        cart.Add(new CartItem("Medium",    10m, 1));

        var top = cart.MostExpensive();
        Assert.NotNull(top);
        Assert.Equal("Expensive", top.Name);
    }

    [Fact]
    public void FibonacciSequence_IsOrdered()
    {
        var math = new MathHelper();
        var fib = math.FibonacciSequence(10).ToList();
        // Starting from index 2, each element >= previous
        for (int i = 2; i < fib.Count; i++)
            Assert.True(fib[i] >= fib[i - 1]);
    }
}
