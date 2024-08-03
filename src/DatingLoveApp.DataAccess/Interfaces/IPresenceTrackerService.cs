namespace DatingLoveApp.DataAccess.Interfaces;

public interface IPresenceTrackerService
{
    Task UserConnectedAsync(string id, string connectedId);
    Task UserDisconnectedAsync(string id, string connectedId);
    Task<List<string>> GetOnlineUsersAsync();
}
