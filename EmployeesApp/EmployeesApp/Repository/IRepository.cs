namespace EmployeesApp.Repository;

public interface IRepository<T> : IDisposable
{
    Task InsertAsync(T data);
    void Update(T data);
    void Delete(T data);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> FindByIdAsync(int? id);
    Task SaveAsync();
}

