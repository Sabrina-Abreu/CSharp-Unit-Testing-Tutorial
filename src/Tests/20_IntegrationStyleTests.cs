using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 20 — Integration-Style Tests
///
/// Integration tests wire real components together (no mocks) to verify
/// that they collaborate correctly end-to-end:
///   - InMemoryProductRepository is a real in-memory implementation
///   - ProductCatalogService uses it without any fakes
///   - Tests exercise a full use-case flow
///
/// These are slower and more fragile than unit tests but catch integration bugs.
/// </summary>
public class IntegrationStyleTests
{
    /// <summary>
    /// Every test gets a fresh repository → no shared state between tests.
    /// </summary>
    private static (ProductCatalogService service, InMemoryProductRepository repo) Build()
    {
        var repo    = new InMemoryProductRepository();
        var service = new ProductCatalogService(repo);
        return (service, repo);
    }

    [Fact]
    public void AddProduct_ThenGetById_ReturnsSameProduct()
    {
        var (service, repo) = Build();

        var added  = service.AddProduct("Laptop", "Electronics", 999.99m, 10);
        var found  = repo.GetById(added.Id);

        Assert.NotNull(found);
        Assert.Equal("Laptop",      found.Name);
        Assert.Equal("Electronics", found.Category);
        Assert.Equal(999.99m,       found.Price);
    }

    [Fact]
    public void Purchase_ReducesStock()
    {
        var (service, repo) = Build();
        var product = service.AddProduct("Mouse", "Electronics", 29.99m, 5);

        bool success = service.Purchase(product.Id, 3);

        Assert.True(success);
        Assert.Equal(2, repo.GetById(product.Id)!.Stock);
    }

    [Fact]
    public void Purchase_MoreThanStock_ReturnsFalse()
    {
        var (service, _) = Build();
        var product = service.AddProduct("Keyboard", "Electronics", 79.99m, 2);

        bool success = service.Purchase(product.Id, 10);

        Assert.False(success);
    }

    [Fact]
    public void Purchase_OutOfStock_ProductIsUnavailable()
    {
        var (service, repo) = Build();
        var product = service.AddProduct("USB Cable", "Accessories", 9.99m, 1);

        service.Purchase(product.Id, 1); // empties stock

        Assert.False(repo.GetById(product.Id)!.IsAvailable);
    }

    [Fact]
    public void GetAvailableByCategory_FiltersCorrectly()
    {
        var (service, _) = Build();
        service.AddProduct("Laptop",   "Electronics", 999m, 5);
        service.AddProduct("Headphones","Electronics", 199m, 0); // out of stock
        service.AddProduct("Desk",     "Furniture",   499m, 3);

        var available = service.GetAvailableByCategory("Electronics").ToList();

        Assert.Single(available);
        Assert.Equal("Laptop", available[0].Name);
    }

    [Fact]
    public void GetInventoryValue_CalculatesCorrectly()
    {
        var (service, _) = Build();
        service.AddProduct("A", "Cat", 10m, 5);  //  50
        service.AddProduct("B", "Cat", 20m, 3);  //  60

        decimal value = service.GetInventoryValue();

        Assert.Equal(110m, value);
    }

    [Fact]
    public void AddProduct_InvalidPrice_ThrowsArgumentOutOfRangeException()
    {
        var (service, _) = Build();
        Assert.Throws<ArgumentOutOfRangeException>(
            () => service.AddProduct("Bad", "X", -1m, 1));
    }

    [Fact]
    public void FullFlow_AddPurchaseCheckInventory()
    {
        var (service, repo) = Build();

        // 1. Add products
        var a = service.AddProduct("Item A", "Widgets", 5m, 10);
        var b = service.AddProduct("Item B", "Widgets", 15m, 4);

        // 2. Purchase
        service.Purchase(a.Id, 4);
        service.Purchase(b.Id, 4);

        // 3. Verify stock
        Assert.Equal(6, repo.GetById(a.Id)!.Stock);
        Assert.Equal(0, repo.GetById(b.Id)!.Stock);

        // 4. Verify inventory value: (5*6) + (15*0) = 30
        Assert.Equal(30m, service.GetInventoryValue());

        // 5. Item B is now unavailable
        var available = service.GetAvailableByCategory("Widgets").ToList();
        Assert.Single(available);
    }
}
