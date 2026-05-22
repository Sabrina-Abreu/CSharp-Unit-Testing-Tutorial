namespace Library;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable => Stock > 0;
}

public interface IProductRepository
{
    Product? GetById(int id);
    IEnumerable<Product> GetAll();
    IEnumerable<Product> GetByCategory(string category);
    void Add(Product product);
    void Update(Product product);
}

/// <summary>
/// In-memory product repository used for integration-style tests (example 20).
/// </summary>
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];
    private int _nextId = 1;

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
    public IEnumerable<Product> GetAll() => _products.AsReadOnly();
    public IEnumerable<Product> GetByCategory(string category) =>
        _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    public void Add(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
    }

    public void Update(Product product)
    {
        var existing = GetById(product.Id)
            ?? throw new KeyNotFoundException($"Product {product.Id} not found.");
        _products.Remove(existing);
        _products.Add(product);
    }
}

/// <summary>
/// Product catalog service used in integration-style tests (example 20).
/// </summary>
public class ProductCatalogService(IProductRepository repository)
{
    public Product AddProduct(string name, string category, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))  throw new ArgumentException("Name required.", nameof(name));
        if (price < 0)  throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        if (stock < 0)  throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative.");

        var product = new Product { Name = name, Category = category, Price = price, Stock = stock };
        repository.Add(product);
        return product;
    }

    public bool Purchase(int productId, int quantity)
    {
        var product = repository.GetById(productId);
        if (product is null || product.Stock < quantity) return false;
        product.Stock -= quantity;
        repository.Update(product);
        return true;
    }

    public IEnumerable<Product> GetAvailableByCategory(string category) =>
        repository.GetByCategory(category).Where(p => p.IsAvailable);

    public decimal GetInventoryValue() =>
        repository.GetAll().Sum(p => p.Price * p.Stock);
}
