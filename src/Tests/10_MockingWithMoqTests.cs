using Library;
using Moq;
using Xunit;

namespace Tests;

/// <summary>
/// Example 10 — Mocking with Moq
///
/// A mock replaces a real dependency with a controllable fake:
///   - Mock&lt;T&gt;.Setup()   : define what the mock returns
///   - Mock&lt;T&gt;.Verify()  : assert that a method was (or was not) called
///   - Times.Once / Times.Never / Times.AtLeast(n)
/// This isolates the unit under test from its collaborators.
/// </summary>
public class MockingWithMoqTests
{
    private readonly Mock<IOrderRepository>    _repoMock  = new();
    private readonly Mock<INotificationService> _notifMock = new();
    private readonly OrderService _service;

    public MockingWithMoqTests()
    {
        _service = new OrderService(_repoMock.Object, _notifMock.Object);
    }

    [Fact]
    public void PlaceOrder_ValidOrder_SavesOrderAndSendsEmail()
    {
        var items = new List<CartItem> { new("Widget", 9.99m, 2) };

        var order = _service.PlaceOrder("alice@example.com", items);

        // Verify repository.Save was called exactly once
        _repoMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Once);

        // Verify an email was sent to the customer
        _notifMock.Verify(
            n => n.SendEmail("alice@example.com", It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);

        Assert.NotNull(order);
        Assert.Equal("alice@example.com", order.CustomerEmail);
    }

    [Fact]
    public void PlaceOrder_EmptyEmail_ThrowsArgumentException()
    {
        var items = new List<CartItem> { new("Widget", 5m, 1) };

        Assert.Throws<ArgumentException>(() => _service.PlaceOrder("", items));

        // Save must NOT be called when validation fails
        _repoMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public void PlaceOrder_EmptyItemList_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.PlaceOrder("bob@example.com", []));
        _repoMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public void CancelOrder_ExistingPendingOrder_ReturnsTrueAndNotifies()
    {
        var order = new Order { Id = 1, CustomerEmail = "carol@test.com", Status = OrderStatus.Pending };
        _repoMock.Setup(r => r.GetById(1)).Returns(order);

        bool result = _service.CancelOrder(1);

        Assert.True(result);
        Assert.Equal(OrderStatus.Cancelled, order.Status);
        _notifMock.Verify(n => n.SendEmail("carol@test.com", It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void CancelOrder_ShippedOrder_ReturnsFalseAndDoesNotNotify()
    {
        var order = new Order { Id = 2, Status = OrderStatus.Shipped };
        _repoMock.Setup(r => r.GetById(2)).Returns(order);

        bool result = _service.CancelOrder(2);

        Assert.False(result);
        _notifMock.Verify(n => n.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void CancelOrder_NonExistingOrder_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetById(999)).Returns((Order?)null);

        bool result = _service.CancelOrder(999);

        Assert.False(result);
    }
}
