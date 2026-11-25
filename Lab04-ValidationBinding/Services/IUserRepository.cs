using Lab04ValidationBinding.Models;

namespace Lab04ValidationBinding.Services;

public interface IUserRepository
{
    IReadOnlyList<User> GetAll();
    User? GetById(int id);
    User Add(User user);
}
