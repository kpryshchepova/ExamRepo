using EmployeesApp.API.Models;

namespace EmployeesApp.API.Repository;

public interface IUserRepository : IDisposable
{
    Task CreateUserAsync(User data);
    Task<User> GetUserAuthDataByNameAsync(string userName);
}

