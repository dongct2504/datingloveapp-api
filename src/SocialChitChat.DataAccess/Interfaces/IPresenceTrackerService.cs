namespace SocialChitChat.DataAccess.Interfaces;

public interface IPresenceTrackerService
{
    Task<bool> UserConnectedAsync(Guid userId, string connectedId);
    Task<bool> UserDisconnectedAsync(Guid userId, string connectedId);
    Task<List<string>> GetOnlineUserIdsAsync();
    Task<List<string>?> GetConnectionIdsForUser(Guid userId);

    Task AddUserToGroupAsync(string groupName, Guid userId);
    Task<List<Guid>> GetGroupMembersAsync(string groupName);
    Task RemoveUserFromGroupAsync(string groupName, Guid userId);
}
