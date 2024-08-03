using DatingLoveApp.DataAccess.Common;
using DatingLoveApp.DataAccess.Interfaces;

namespace DatingLoveApp.DataAccess.Services;

public class PresenceTrackerService : IPresenceTrackerService
{
    private readonly ICacheService _cacheService;

    public PresenceTrackerService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task UserConnectedAsync(string id, string connectedId)
    {
        string key = $"{CacheConstants.Presence}-{id}";
        List<string>? connectedIds = await _cacheService.GetAsync<List<string>>(key);

        if (connectedIds == null)
        {
            connectedIds = new List<string>();
        }

        connectedIds.Add(connectedId);
        await _cacheService.SetAsync(key, connectedIds, CacheOptions.PresenceExpiration);
    }

    public async Task UserDisconnectedAsync(string id, string connectedId)
    {
        string key = $"{CacheConstants.Presence}-{id}";
        List<string>? connectedIds = await _cacheService.GetAsync<List<string>>(key);

        if (connectedIds == null)
        {
            return;
        }

        connectedIds.Remove(connectedId);

        if (!connectedIds.Any())
        {
            await _cacheService.RemoveAsync(key);
            return;
        }

        await _cacheService.SetAsync(
            key,
            connectedIds,
            CacheOptions.PresenceExpiration);
    }

    public async Task<List<string>> GetOnlineUsersAsync()
    {
        return await _cacheService.GetKeysExcepPrefixAsync(CacheConstants.Presence);
    }
}
