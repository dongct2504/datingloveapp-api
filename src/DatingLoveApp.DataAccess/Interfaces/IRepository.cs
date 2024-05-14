using DatingLoveApp.DataAccess.Extensions;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(QueryOptions<T>? options = null, bool asNoTracking = false);

    Task<int> GetCountAsync();

    Task<T?> GetAsync(QueryOptions<T> options, bool asNoTracking = false);

    Task AddAsync(T entity);
    Task RemoveAsync(T entity);
}
