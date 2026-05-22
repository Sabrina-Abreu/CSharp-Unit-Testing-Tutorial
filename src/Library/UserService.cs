namespace Library;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastLogin { get; set; }
}

public interface IUserRepository
{
    User? FindById(int id);
    User? FindByEmail(string email);
    IEnumerable<User> GetAll();
    void Add(User user);
}

/// <summary>
/// User service used to demonstrate null tests and mocking with Moq.
/// </summary>
public class UserService(IUserRepository repository)
{
    public User? GetUser(int id) => repository.FindById(id);

    public User? GetUserByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;
        return repository.FindByEmail(email);
    }

    public IEnumerable<User> GetActiveUsers() =>
        repository.GetAll().Where(u => u.IsActive);

    public void RegisterUser(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        repository.Add(new User { Name = name, Email = email, IsActive = true });
    }
}
