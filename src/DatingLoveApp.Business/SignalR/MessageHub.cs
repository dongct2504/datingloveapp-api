using DatingLoveApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingLoveApp.Business.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMessageRepository _messageRepository;

    public MessageHub()
    {
    }
}
