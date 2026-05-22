using Library;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Tests;

/// <summary>
/// Example 11 — Dependency Injection Tests
///
/// Testing that services wired with Microsoft.Extensions.DependencyInjection
/// resolve correctly and behave as expected. You can mix real and fake services:
///   - Register mocks inside the DI container for controlled behaviour
///   - Resolve the SUT from the container just like production code
/// </summary>
public class DependencyInjectionTests
{
    /// <summary>
    /// Builds a ServiceCollection that mirrors the production setup
    /// but replaces external dependencies with Moq fakes.
    /// </summary>
    private static (ServiceProvider provider,
                    Mock<IOrderRepository>    repoMock,
                    Mock<INotificationService> notifMock)
        BuildProvider()
    {
        var repoMock  = new Mock<IOrderRepository>();
        var notifMock = new Mock<INotificationService>();

        var services = new ServiceCollection();
        services.AddSingleton(repoMock.Object);
        services.AddSingleton(notifMock.Object);
        services.AddTransient<OrderService>();

        return (services.BuildServiceProvider(), repoMock, notifMock);
    }

    [Fact]
    public void OrderService_ResolvesFromContainer()
    {
        var (provider, _, _) = BuildProvider();
        var service = provider.GetRequiredService<OrderService>();
        Assert.NotNull(service);
    }

    [Fact]
    public void OrderService_ResolvedFromContainer_PlacesOrder()
    {
        var (provider, repoMock, _) = BuildProvider();
        var service = provider.GetRequiredService<OrderService>();
        var items   = new List<CartItem> { new("Book", 29.99m, 1) };

        service.PlaceOrder("user@example.com", items);

        repoMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public void Transient_OrderService_CreatesNewInstanceEachTime()
    {
        var (provider, _, _) = BuildProvider();
        var s1 = provider.GetRequiredService<OrderService>();
        var s2 = provider.GetRequiredService<OrderService>();

        // Transient = new instance every time
        Assert.NotSame(s1, s2);
    }

    [Fact]
    public void Singleton_RepoMock_ReturnsSameInstanceEachTime()
    {
        var (provider, _, _) = BuildProvider();
        var r1 = provider.GetRequiredService<IOrderRepository>();
        var r2 = provider.GetRequiredService<IOrderRepository>();

        // Singleton = same instance every time
        Assert.Same(r1, r2);
    }

    [Fact]
    public void UserService_WithFakeRepo_WorksEndToEnd()
    {
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(r => r.FindById(42))
                    .Returns(new User { Id = 42, Name = "DI User", IsActive = true });

        var services = new ServiceCollection();
        services.AddSingleton(userRepoMock.Object);
        services.AddTransient<UserService>();
        var provider = services.BuildServiceProvider();

        var userService = provider.GetRequiredService<UserService>();
        var user        = userService.GetUser(42);

        Assert.NotNull(user);
        Assert.Equal("DI User", user.Name);
    }
}
