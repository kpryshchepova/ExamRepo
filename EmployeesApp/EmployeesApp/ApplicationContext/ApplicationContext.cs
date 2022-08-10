using Microsoft.EntityFrameworkCore;
using EmployeesApp.Models;

namespace EmployeesApp.ApplicationContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Todo> Todos { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
