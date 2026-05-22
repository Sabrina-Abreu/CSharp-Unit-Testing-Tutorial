namespace Library;

public enum OrderStatus { Pending, Processing, Shipped, Delivered, Cancelled }

public class Order
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public List<CartItem> Items { get; set; } = [];
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Total => Items.Sum(i => i.Price * i.Quantity);
}

public interface IOrderRepository
{
    Order? GetById(int id);
    void Save(Order order);
    IEnumerable<Order> GetByCustomerEmail(string email);
}

public interface INotificationService
{
    void SendEmail(string to, string subject, string body);
    void SendSms(string phoneNumber, string message);
    Task SendEmailAsync(string to, string subject, string body);
}

/// <summary>
/// Order service used to demonstrate mocking with Moq and DI tests.
/// </summary>
public class OrderService(IOrderRepository repository, INotificationService notifications)
{
    public Order? GetOrder(int id) => repository.GetById(id);

    public Order PlaceOrder(string customerEmail, List<CartItem> items)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException("Customer email is required.", nameof(customerEmail));
        if (items is null || items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.", nameof(items));

        var order = new Order { CustomerEmail = customerEmail, Items = items };
        repository.Save(order);
        notifications.SendEmail(customerEmail, "Order Confirmed",
            $"Your order #{order.Id} totalling {order.Total:C} has been placed.");
        return order;
    }

    public async Task<Order> PlaceOrderAsync(string customerEmail, List<CartItem> items)
    {
        var order = PlaceOrder(customerEmail, items);
        await notifications.SendEmailAsync(customerEmail, "Order Confirmed",
            $"Your order #{order.Id} has been placed.");
        return order;
    }

    public bool CancelOrder(int orderId)
    {
        var order = repository.GetById(orderId);
        if (order is null || order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered)
            return false;
        order.Status = OrderStatus.Cancelled;
        repository.Save(order);
        notifications.SendEmail(order.CustomerEmail, "Order Cancelled",
            $"Order #{orderId} has been cancelled.");
        return true;
    }
}
