namespace Library;

public record CartItem(string Name, decimal Price, int Quantity);

/// <summary>
/// Shopping cart used to demonstrate collection-based assertions.
/// </summary>
public class ShoppingCart
{
    private readonly List<CartItem> _items = [];

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public bool IsEmpty => _items.Count == 0;
    public decimal Total => _items.Sum(i => i.Price * i.Quantity);

    public void Add(CartItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(item));
        _items.Add(item);
    }

    public void Remove(string name) => _items.RemoveAll(i => i.Name == name);
    public void Clear() => _items.Clear();

    public decimal ApplyDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
        return Total * (1 - percentage / 100);
    }

    public CartItem? MostExpensive() =>
        _items.OrderByDescending(i => i.Price).FirstOrDefault();
}
