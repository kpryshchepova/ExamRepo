using Microsoft.EntityFrameworkCore;
using EmployeesApp.API.Models;

namespace EmployeesApp.API.ApplicationContext;

public class ApplicationContext : DbContext
{
    public DbSet<User> UsersData { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

}

