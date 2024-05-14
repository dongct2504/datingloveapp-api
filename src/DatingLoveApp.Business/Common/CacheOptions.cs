using Microsoft.Extensions.Caching.Distributed;

namespace DatingLoveApp.Business.Common;

public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration =>
        new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) };
}
