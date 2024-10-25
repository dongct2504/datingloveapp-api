using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialChitChat.Business.Common;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.MessageDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.Business.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IPresenceTrackerService _presenceTrackerService;

    public MessageHub(IMessageService messageService, IPresenceTrackerService presenceTrackerService)
    {
        _messageService = messageService;
        _presenceTrackerService = presenceTrackerService;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        if (httpContext != null && Context.User != null)
        {
            Guid currentUserId = Context.User.GetCurrentUserId();
            Guid otherUserId = Guid.Parse(httpContext.Request.Query["otherId"].ToString());

            string groupName = Utils.GetGroupName(new Guid[] { currentUserId, otherUserId });
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await _presenceTrackerService.AddUserToGroupAsync(groupName, currentUserId);

            Result<PagedList<MessageDto>> result = await _messageService
                .GetMessagesBetweenParticipantsAsync(new GetMessageBetweenParticipantsParams
                {
                    CurrentUserId = currentUserId,
                    RecipientId = otherUserId,
                    PageNumber = 1,
                    PageSize = 30
                });
            if (result.IsFailed)
            {
                throw new HubException(result.Errors.First().Message);
            }
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", result.Value);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();

        if (httpContext != null && Context.User != null)
        {
            Guid currentUserId = Context.User.GetCurrentUserId();
            Guid otherUserId = Guid.Parse(httpContext.Request.Query["otherId"].ToString());

            string groupName = Utils.GetGroupName(new Guid[] { currentUserId, otherUserId });
            await _presenceTrackerService.RemoveUserFromGroupAsync(groupName, currentUserId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(SendMessageDto createMessageDto)
    {
        if (Context.User == null)
        {
            return;
        }

        createMessageDto.SenderId = Context.User.GetCurrentUserId();

        Result<MessageDto> result = await _messageService.SendMessageToRecipientAsync(createMessageDto);
        if (result.IsFailed)
        {
            throw new HubException(result.Errors.First().Message);
        }

        string groupName = Utils.GetGroupName(new Guid[]
        {
            createMessageDto.SenderId, createMessageDto.RecipientId
        });
        await Clients.Group(groupName).SendAsync("NewMessage", result.Value);
    }
}
