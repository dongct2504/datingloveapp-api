using DatingLoveApp.DataAccess.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingLoveApp.DataAccess.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Others.SendAsync("UserIsOnline", Context.User?.GetCurrentUserId());
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetCurrentUserId());

        await base.OnDisconnectedAsync(exception);
    }
}
