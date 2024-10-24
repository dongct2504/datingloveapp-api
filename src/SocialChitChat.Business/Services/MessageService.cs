using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SocialChitChat.Business.Common;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.MessageDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.Business.SignalR;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.Business.Services;

public class MessageService : IMessageService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPresenceTrackerService _presenceTrackerService;
    private readonly ICacheService _cacheService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IHubContext<PresenceHub> _presenceHubContext;
    private readonly IMapper _mapper;

    public MessageService(
        UserManager<AppUser> userManager,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper,
        SocialChitChatDbContext dbContext,
        ICacheService cacheService,
        IPresenceTrackerService presenceTrackerService,
        IHubContext<PresenceHub> presenceHubContext,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
        _dbContext = dbContext;
        _cacheService = cacheService;
        _presenceTrackerService = presenceTrackerService;
        _presenceHubContext = presenceHubContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<MessageDto>> SendMessageToRecipientAsync(SendMessageDto sendMessageDto)
    {
        AppUser? sender = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.Pictures)
            .FirstOrDefaultAsync(u => u.Id == sendMessageDto.SenderId);
        if (sender == null)
        {
            Log.Warning($"{nameof(SendMessageToRecipientAsync)} - {ErrorMessageConsts.SenderNotFound} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.SenderNotFound));
        }

        AppUser? recipient = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == sendMessageDto.RecipientId);
        if (recipient == null)
        {
            Log.Warning($"{nameof(SendMessageToRecipientAsync)} - {ErrorMessageConsts.RecipientNotFound} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.RecipientNotFound));
        }

        if (sender.Id == recipient.Id)
        {
            Log.Warning($"{nameof(SendMessageToRecipientAsync)} - {ErrorMessageConsts.LikeYourselfError} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.LikeYourselfError));
        }

        Conversation? conversation = await _dbContext.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c =>
                c.Participants.Any(p => p.AppUserId == sender.Id) &&
                c.Participants.Any(p => p.AppUserId == recipient.Id) &&
                !c.IsGroupChat);
        
        if (conversation == null)
        {
            conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                IsGroupChat = false,
                CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
            };
            _unitOfWork.Conversations.Add(conversation);

            List<Participant> participants = new List<Participant>
            {
                new Participant
                {
                    ConversationId = conversation.Id,
                    AppUserId = sender.Id,
                    JoinAt = _dateTimeProvider.LocalVietnamDateTimeNow
                },
                new Participant
                {
                    ConversationId = conversation.Id,
                    AppUserId = recipient.Id,
                    JoinAt = _dateTimeProvider.LocalVietnamDateTimeNow
                }
            };
            _unitOfWork.Participants.AddRange(participants);
        }

        Message message = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            SenderId = sender.Id,
            Content = sendMessageDto.Content,
            MessageSent = _dateTimeProvider.LocalVietnamDateTimeNow,
        };

        string groupName = Utils.GetGroupName(new Guid[] { sender.Id, recipient.Id });
        List<Guid> groupUsers = await _presenceTrackerService.GetGroupMembersAsync(groupName);

        if (groupUsers.Contains(recipient.Id))
        {
            message.DateRead = _dateTimeProvider.LocalVietnamDateTimeNow;
        }
        else
        {
            List<string>? connectionIds = await _presenceTrackerService.GetConnectionIdsForUser(recipient.Id);
            if (connectionIds != null)
            {
                await _presenceHubContext.Clients.Clients(connectionIds).SendAsync("NewMessageReceived",
                    new
                    {
                        senderId = sender.Id,
                        senderNickName = sender.Nickname
                    });
            }
        }

        _unitOfWork.Messages.Add(message);
        conversation.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;

        await _unitOfWork.SaveChangesAsync();

        MessageDto messageDto = new MessageDto
        {
            Id = message.Id,
            ConversationId = conversation.Id,
            SenderId = sender.Id,
            SenderNickName = sender.Nickname,
            SenderImageUrl = sender.Pictures
                .Where(p => p.IsMain)
                .Select(p => p.ImageUrl)
                .FirstOrDefault(),
            Content = message.Content,
            MessageSent = message.MessageSent,
            DateRead = message.DateRead
        };

        return messageDto;
    }

    public async Task<Result<MessageDto>> ChangeToReadAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MessageDto>> GetMessageThreadAsync(Guid currentUserId, Guid recipientId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteMessageAsync(Guid userId, Guid messageId)
    {
        throw new NotImplementedException();
    }
}
