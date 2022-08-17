using EmployeesApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApp.API.Repository;

public class UserRepository : IUserRepository
{
    private ApplicationContext.ApplicationContext _db;

    public UserRepository(ApplicationContext.ApplicationContext applicationContext)
    {
        _db = applicationContext;
    }

    public async Task CreateUserAsync(User user)
    {
        await _db.UsersData.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User> GetUserAuthDataByNameAsync(string userName)
    {
        return await _db.UsersData.FirstOrDefaultAsync(item => item.UserName == userName);
    }

    private bool disposed = false;
    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

