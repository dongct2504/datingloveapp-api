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
            bool isOnline = await _presenceTrackerService
                .UserConnectedAsync(Context.User.GetCurrentUserId(), Context.ConnectionId);
            if (isOnline)
            {
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetCurrentUserId());
            }

            List<string> currentUsers = await _presenceTrackerService.GetOnlineUserIdsAsync();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User != null)
        {
            bool isOffline = await _presenceTrackerService
                .UserDisconnectedAsync(Context.User.GetCurrentUserId(), Context.ConnectionId);
            if (isOffline)
            {
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetCurrentUserId());
            }
        }
        await base.OnDisconnectedAsync(exception);
    }
}
