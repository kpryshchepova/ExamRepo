using EmployeesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApp.Repository;

public class EmployeeRepository : IRepository<Employee>
{
    private ApplicationContext.ApplicationContext _db;

    public EmployeeRepository(ApplicationContext.ApplicationContext applicationContext)
    {
        _db = applicationContext;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _db.Employees.ToListAsync();
    }

    public async Task InsertAsync(Employee data)
    {
        await _db.Employees.AddAsync(data);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<Employee> FindByIdAsync(int? id)
    {
        return await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(Employee data)
    {
        _db.Employees.Update(data);
    }

    public void Delete(Employee data)
    {
        _db.Entry(data).State = EntityState.Deleted;

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

