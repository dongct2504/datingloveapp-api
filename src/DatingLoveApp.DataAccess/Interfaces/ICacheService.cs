using Microsoft.Extensions.Caching.Distributed;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface ICacheService
{
    Task<List<T>> GetByPrefixAsync<T>(
        string prefixKey,
        CancellationToken cancellationToken = default) where T : class;

    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    Task SetAsync<T>(
        string key,
        T value,
        DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default) where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}
