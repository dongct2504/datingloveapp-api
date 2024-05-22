﻿using DatingLoveApp.DataAccess.Extensions;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(QueryOptions<T>? options = null, bool asNoTracking = false);

    Task<int> GetCountAsync();

    Task<T?> GetAsync(int id);
    Task<T?> GetAsync(long id);
    Task<T?> GetAsync(Guid id);
    Task<T?> GetAsync(QueryOptions<T> options, bool asNoTracking = false);

    Task AddAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveAllAsync();
}
