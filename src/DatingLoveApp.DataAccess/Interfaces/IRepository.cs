using DatingLoveApp.DataAccess.Specifications;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = false);

    Task<int> GetCountAsync();

    Task<T?> GetAsync(int id);
    Task<T?> GetAsync(long id);
    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(Guid id);

    Task AddAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveAllAsync();

    Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false);
    Task<T?> GetWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false);
}
