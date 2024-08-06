using DatingLoveApp.Business.Dtos.MessageDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingLoveApp.Business.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMessageService _messageService;

    public MessageHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        if (httpContext != null && Context.User != null)
        {
            string currentUserId = Context.User.GetCurrentUserId();
            string otherUserId = httpContext.Request.Query["otherId"].ToString();
            string groupName = GetGroupName(currentUserId, otherUserId);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await _messageService.AddUserToGroupAsync(groupName, currentUserId);

            List<MessageDto> messages = await _messageService.GetMessageThreadAsync(currentUserId, otherUserId);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();

        if (httpContext != null && Context.User != null)
        {
            string currentUserId = Context.User.GetCurrentUserId();
            string otherUserId = httpContext.Request.Query["otherId"].ToString();
            string groupName = GetGroupName(currentUserId, otherUserId);
            await _messageService.RemoveUserFromGroupAsync(groupName, currentUserId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(CreateMessageDto createMessageDto)
    {
        if (Context.User == null)
        {
            return;
        }

        createMessageDto.UserId = Context.User.GetCurrentUserId();

        Result<MessageDto> result = await _messageService.CreateMessageAsync(createMessageDto);
        if (result.IsFailed)
        {
            throw new HubException(result.Errors.First().Message);
        }

        string groupName = GetGroupName(result.Value.SenderId, result.Value.RecipientId);
        await Clients.Group(groupName).SendAsync("NewMessage", result.Value);
    }

    private string GetGroupName(string callerId, string otherId)
    {
        bool stringCompare = string.CompareOrdinal(callerId, otherId) < 0;
        return stringCompare ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";
    }
}
