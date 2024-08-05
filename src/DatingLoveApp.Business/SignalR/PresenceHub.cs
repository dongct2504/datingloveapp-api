using DatingLoveApp.DataAccess.Extensions;
using DatingLoveApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingLoveApp.Business.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    private readonly IPresenceTrackerService _presenceTrackerService;

    public PresenceHub(IPresenceTrackerService presenceTrackerService)
    {
        _presenceTrackerService = presenceTrackerService;
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.User != null)
        {
            await _presenceTrackerService.UserConnectedAsync(Context.User.GetCurrentUserId(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetCurrentUserId());

            List<string> currentUsers = await _presenceTrackerService.GetOnlineUsersAsync();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User != null)
        {
            await _presenceTrackerService
                .UserDisconnectedAsync(Context.User.GetCurrentUserId(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetCurrentUserId());

            List<string> currentUsers = await _presenceTrackerService.GetOnlineUsersAsync();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
