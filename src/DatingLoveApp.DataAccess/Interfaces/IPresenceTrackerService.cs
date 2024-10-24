namespace DatingLoveApp.DataAccess.Interfaces;

public interface IPresenceTrackerService
{
    Task<bool> UserConnectedAsync(string id, string connectedId);
    Task<bool> UserDisconnectedAsync(string id, string connectedId);
    Task<List<string>> GetOnlineUserIdsAsync();
    Task<List<string>?> GetConnectionIdsForUser(string id);
}
