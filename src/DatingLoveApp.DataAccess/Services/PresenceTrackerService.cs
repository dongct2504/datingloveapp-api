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
        List<string>? connectedIds = await _cacheService.GetAsync<List<string>>(id);

        if (connectedIds == null)
        {
            await _cacheService.SetAsync(
                $"{CacheConstants.Presence}-{id}",
                new List<string> { connectedId },
                CacheOptions.PresenceExpiration);
            return;
        }

        connectedIds.Add(connectedId);
        await _cacheService.SetAsync(id, connectedIds, CacheOptions.PresenceExpiration);
    }

    public async Task UserDisconnectedAsync(string id, string connectedId)
    {
        List<string>? connectedIds = await _cacheService
            .GetAsync<List<string>>($"{CacheConstants.Presence}-{id}");

        if (connectedIds == null)
        {
            return;
        }

        connectedIds.Remove(connectedId);

        if (!connectedIds.Any())
        {
            await _cacheService.RemoveAsync(id);
            return;
        }

        await _cacheService.SetAsync(
            $"{CacheConstants.Presence}-{id}",
            connectedIds,
            CacheOptions.PresenceExpiration);
    }

    public async Task<List<string>> GetOnlineUsersAsync()
    {
        return await _cacheService.GetByPrefixAsync<string>(CacheConstants.Presence);
    }
}
