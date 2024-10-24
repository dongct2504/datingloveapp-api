using SocialChitChat.DataAccess.Common;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Services;

public class PresenceTrackerService : IPresenceTrackerService
{
    private readonly ICacheService _cacheService;

    public PresenceTrackerService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<bool> UserConnectedAsync(Guid id, string connectedId)
    {
        bool isOnline = false;

        string key = $"{CacheConstants.Presence}-{id}";

        List<string>? connectedIds = await _cacheService.GetAsync<List<string>>(key);
        if (connectedIds != null)
        {
            // already online so no need to update the current online users
            connectedIds.Add(connectedId);
        }
        else
        {
            connectedIds = new List<string> { connectedId };
            isOnline = true;
        }

        await _cacheService.SetAsync(key, connectedIds, CacheOptions.PresenceExpiration);
        return isOnline;
    }

    public async Task<bool> UserDisconnectedAsync(Guid userId, string connectedId)
    {
        bool isOffline = false;

        string key = $"{CacheConstants.Presence}-{userId}";
        List<string>? connectedIds = await _cacheService.GetAsync<List<string>>(key);

        if (connectedIds == null)
        {
            return isOffline;
        }

        connectedIds.Remove(connectedId);

        if (!connectedIds.Any())
        {
            await _cacheService.RemoveAsync(key);
            isOffline = true;
            return isOffline;
        }

        await _cacheService.SetAsync(
            key,
            connectedIds,
            CacheOptions.PresenceExpiration);

        return isOffline;
    }

    public async Task<List<string>> GetOnlineUserIdsAsync()
    {
        return await _cacheService.GetKeysExcepPrefixAsync(CacheConstants.Presence);
    }

    public async Task<List<string>?> GetConnectionIdsForUser(Guid userId)
    {
        string key = $"{CacheConstants.Presence}-{userId}";
        List<string>? connectedIds = await _cacheService.GetAsync<List<string>>(key);
        return connectedIds;
    }

    // grouping for message thread
    public async Task AddUserToGroupAsync(string groupName, Guid userId)
    {
        string groupKey = $"group-{groupName}";
        List<Guid> groupMembers = await _cacheService.GetAsync<List<Guid>>(groupKey) ?? new List<Guid>();
        groupMembers.Add(userId);

        await _cacheService.SetAsync(groupKey, groupMembers, CacheOptions.PresenceExpiration);
    }

    public async Task<List<Guid>> GetGroupMembersAsync(string groupName)
    {
        string groupKey = $"group-{groupName}";
        return await _cacheService.GetAsync<List<Guid>>(groupKey) ?? new List<Guid>();
    }

    public async Task RemoveUserFromGroupAsync(string groupName, Guid userId)
    {
        string groupKey = $"group-{groupName}";
        List<Guid>? groupMembers = await _cacheService.GetAsync<List<Guid>>(groupKey);
        if (groupMembers != null)
        {
            groupMembers.Remove(userId);

            if (!groupMembers.Any())
            {
                await _cacheService.RemoveAsync(groupKey);
            }
            else
            {
                await _cacheService.SetAsync(groupKey, groupMembers, CacheOptions.PresenceExpiration);
            }
        }
    }
}
