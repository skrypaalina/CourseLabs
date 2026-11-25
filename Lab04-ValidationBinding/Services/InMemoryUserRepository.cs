using Lab04ValidationBinding.Models;

namespace Lab04ValidationBinding.Services;

public class InMemoryUserRepository : IUserRepository
{
    private readonly object _lock = new();
    private readonly List<User> _users =
    [
        new() { Id = 1, Name = "Alice Johnson", Email = "alice@example.com" },
        new() { Id = 2, Name = "David Smith", Email = "david@example.com" },
        new() { Id = 3, Name = "Maria Brown", Email = "maria@example.com" }
    ];
    private int _nextId = 4;

    public IReadOnlyList<User> GetAll()
    {
        lock (_lock) return _users.Select(Copy).ToList();
    }

    public User? GetById(int id)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user is null ? null : Copy(user);
        }
    }

    public User Add(User user)
    {
        lock (_lock)
        {
            user.Id = _nextId++;
            _users.Add(Copy(user));
            return Copy(user);
        }
    }

    private static User Copy(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };
}
