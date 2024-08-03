using Microsoft.Extensions.Caching.Distributed;

namespace DatingLoveApp.DataAccess.Common;

public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration =>
        new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) };

    public static DistributedCacheEntryOptions PresenceExpiration =>
        new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2) };
}
