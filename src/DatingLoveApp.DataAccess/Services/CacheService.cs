using DatingLoveApp.DataAccess.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DatingLoveApp.DataAccess.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedValue == null)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(cachedValue);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default)
        where T : class
    {
        string cacheValue = JsonConvert.SerializeObject(value);

        await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
