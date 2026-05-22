using Library;
using Moq;
using Xunit;

namespace Tests;

/// <summary>
/// Example 07 — Null Tests
///
/// Verifying that methods correctly handle and signal absent values:
///   - Assert.Null    : value must be null
///   - Assert.NotNull : value must not be null
/// The IUserRepository is faked with Moq to control return values.
/// </summary>
public class NullTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly UserService _service;

    public NullTests()
    {
        _service = new UserService(_repoMock.Object);
    }

    [Fact]
    public void GetUser_ExistingId_ReturnsUser()
    {
        _repoMock.Setup(r => r.FindById(1))
                 .Returns(new User { Id = 1, Name = "Alice" });

        var user = _service.GetUser(1);

        Assert.NotNull(user);         // must NOT be null
        Assert.Equal("Alice", user.Name);
    }

    [Fact]
    public void GetUser_NonExistingId_ReturnsNull()
    {
        _repoMock.Setup(r => r.FindById(999)).Returns((User?)null);

        var user = _service.GetUser(999);

        Assert.Null(user);            // must be null
    }

    [Fact]
    public void GetUserByEmail_BlankEmail_ReturnsNull()
    {
        // No setup needed — blank email is short-circuited in UserService
        var user = _service.GetUserByEmail("   ");
        Assert.Null(user);
    }

    [Fact]
    public void GetActiveUsers_NoActiveUsers_ReturnsEmptyEnumerable()
    {
        _repoMock.Setup(r => r.GetAll()).Returns([]);

        var users = _service.GetActiveUsers();

        Assert.NotNull(users);         // the enumerable itself must not be null
        Assert.Empty(users);
    }

    [Fact]
    public void GetActiveUsers_MixedUsers_ReturnsOnlyActive()
    {
        _repoMock.Setup(r => r.GetAll()).Returns(
        [
            new User { Id = 1, IsActive = true,  Name = "Alice" },
            new User { Id = 2, IsActive = false, Name = "Bob"   },
            new User { Id = 3, IsActive = true,  Name = "Carol" },
        ]);

        var active = _service.GetActiveUsers().ToList();

        Assert.Equal(2, active.Count);
        Assert.All(active, u => Assert.NotNull(u.Name));
    }

    [Fact]
    public void MostExpensive_EmptyCart_ReturnsNull()
    {
        var cart = new ShoppingCart();
        CartItem? top = cart.MostExpensive();
        Assert.Null(top);
    }
}
