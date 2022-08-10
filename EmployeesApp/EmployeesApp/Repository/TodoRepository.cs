using EmployeesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApp.Repository;

public class TodoRepository : IRepository<Todo>
{
    private ApplicationContext.ApplicationContext _db;

    public TodoRepository(ApplicationContext.ApplicationContext applicationContext)
    {
        _db = applicationContext;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await _db.Todos.ToListAsync();
    }

    public async Task InsertAsync(Todo data)
    {
        await _db.Todos.AddAsync(data);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<Todo> FindByIdAsync(int? id)
    {
        return await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(Todo data)
    {
        _db.Todos.Update(data);
    }

    public void Delete(Todo data)
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


