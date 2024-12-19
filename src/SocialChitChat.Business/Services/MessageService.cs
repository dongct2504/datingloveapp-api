using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialChitChat.Business.Common;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.MessageDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.Business.SignalR;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;
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
    private readonly ILogger<MessageService> _logger;
    private readonly IMapper _mapper;

    public MessageService(
        UserManager<AppUser> userManager,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper,
        SocialChitChatDbContext dbContext,
        ICacheService cacheService,
        IPresenceTrackerService presenceTrackerService,
        IHubContext<PresenceHub> presenceHubContext,
        IUnitOfWork unitOfWork,
        ILogger<MessageService> logger)
    {
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
        _dbContext = dbContext;
        _cacheService = cacheService;
        _presenceTrackerService = presenceTrackerService;
        _presenceHubContext = presenceHubContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PagedList<MessageDto>>> GetMessagesForUserAsync(MessageParams messageParams)
    {
        AppUser? sender = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == messageParams.UserId);
        if (sender == null)
        {
            _logger.LogWarning($"{nameof(SendMessageToRecipientAsync)} - {ErrorMessageConsts.SenderNotFound} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.SenderNotFound));
        }

        IQueryable<Message> messagesQuery = _dbContext.Messages.AsQueryable();

        messagesQuery = messagesQuery
            .Where(m => m.SenderId == sender.Id);

        int totalItems = await messagesQuery.CountAsync();

        List<MessageDto> messageDtos = await messagesQuery
            .AsNoTracking()
            .OrderByDescending(m => m.MessageSent) // get the latest messages first to paging
            .Skip((messageParams.PageNumber - 1) * messageParams.PageSize)
            .Take(messageParams.PageSize)
            .ProjectToType<MessageDto>()
            .ToListAsync();

        PagedList<MessageDto> pagedList = new PagedList<MessageDto>
        {
            TotalRecords = totalItems,
            Items = messageDtos,
            PageNumber = messageParams.PageNumber,
            PageSize = messageParams.PageSize
        };

        return pagedList;
    }

    public async Task<Result<PagedList<MessageDto>>> GetMessagesBetweenParticipantsAsync(GetMessageBetweenParticipantsParams participantsParams)
    {
        AppUser? sender = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.Pictures)
            .FirstOrDefaultAsync(u => u.Id == participantsParams.CurrentUserId);
        if (sender == null)
        {
            _logger.LogWarning($"{nameof(GetMessagesBetweenParticipantsAsync)} - {ErrorMessageConsts.SenderNotFound} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.SenderNotFound));
        }

        AppUser? recipient = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == participantsParams.RecipientId);
        if (recipient == null)
        {
            _logger.LogWarning($"{nameof(GetMessagesBetweenParticipantsAsync)} - {ErrorMessageConsts.RecipientNotFound} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.RecipientNotFound));
        }

        GroupChat? conversation = await _dbContext.GroupChats
            .Include(u => u.Participants)
            .FirstOrDefaultAsync(u =>
                u.Participants.Any(p => p.AppUserId == sender.Id) &&
                u.Participants.Any(p => p.AppUserId == recipient.Id) &&
                !u.IsGroupChat);

        if (conversation == null)
        {
            return new PagedList<MessageDto>
            {
                TotalRecords = 0,
                Items = new List<MessageDto>(),
                PageNumber = participantsParams.PageNumber,
                PageSize = participantsParams.PageSize
            };
        }

        int totalItems = await _dbContext.Messages
            .Where(m => m.GroupChatId == conversation.Id)
            .CountAsync();

        List<MessageDto> messageDtos = await _dbContext.Messages
            .AsNoTracking()
            .Where(m => m.GroupChatId == conversation.Id)
            .OrderByDescending(m => m.MessageSent) // get the latest messages first to paging
            .Skip((participantsParams.PageNumber - 1) * participantsParams.PageSize)
            .Take(participantsParams.PageSize)
            .ProjectToType<MessageDto>()
            .ToListAsync();

        messageDtos.Reverse(); // get the latest message last for frontend

        PagedList<MessageDto> pagedList = new PagedList<MessageDto>
        {
            TotalRecords = totalItems,
            Items = messageDtos,
            PageNumber = participantsParams.PageNumber,
            PageSize = participantsParams.PageSize
        };

        return pagedList;
    }

    public async Task<Result<MessageDto>> SendMessageToRecipientAsync(SendMessageDto sendMessageDto)
    {
        AppUser? sender = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.Pictures)
            .FirstOrDefaultAsync(u => u.Id == sendMessageDto.SenderId);
        if (sender == null)
        {
            _logger.LogWarning($"{nameof(SendMessageToRecipientAsync)} - {ErrorMessageConsts.SenderNotFound} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.SenderNotFound));
        }

        AppUser? recipient = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == sendMessageDto.RecipientId);
        if (recipient == null)
        {
            _logger.LogWarning($"{nameof(SendMessageToRecipientAsync)} - {ErrorMessageConsts.RecipientNotFound} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.RecipientNotFound));
        }

        if (sender.Id == recipient.Id)
        {
            _logger.LogWarning($"{nameof(SendMessageToRecipientAsync)} - {ErrorMessageConsts.ChatYourselfError} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.ChatYourselfError));
        }

        GroupChat? conversation = await _dbContext.GroupChats
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c =>
                c.Participants.Any(p => p.AppUserId == sender.Id) &&
                c.Participants.Any(p => p.AppUserId == recipient.Id) &&
                !c.IsGroupChat);

        if (conversation == null)
        {
            conversation = new GroupChat
            {
                Id = Guid.NewGuid(),
                IsGroupChat = false,
                CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
            };
            _unitOfWork.GroupChats.Add(conversation);

            List<Participant> participants = new List<Participant>
            {
                new Participant
                {
                    GroupChatId = conversation.Id,
                    AppUserId = sender.Id,
                    JoinAt = _dateTimeProvider.LocalVietnamDateTimeNow
                },
                new Participant
                {
                    GroupChatId = conversation.Id,
                    AppUserId = recipient.Id,
                    JoinAt = _dateTimeProvider.LocalVietnamDateTimeNow
                }
            };
            _unitOfWork.Participants.AddRange(participants);
        }

        Message message = new Message
        {
            Id = Guid.NewGuid(),
            GroupChatId = conversation.Id,
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
            SenderImageUrl = sender.GetMainProfilePictureUrl(),
            Content = message.Content,
            MessageSent = message.MessageSent,
            DateRead = message.DateRead
        };

        return messageDto;
    }

    public async Task<Result<MessageDto>> ChangeToReadAsync(Guid id)
    {
        Message? message = await _dbContext.Messages
            .Include(m => m.AppUser)
            .ThenInclude(u => u.Pictures)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (message == null)
        {
            _logger.LogWarning($"{nameof(ChangeToReadAsync)} - {ErrorMessageConsts.MessageNotFound} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.MessageNotFound));
        }

        message.DateRead = _dateTimeProvider.LocalVietnamDateTimeNow;
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MessageDto>(message);
    }

    public async Task<Result> DeleteMessageAsync(Guid messageId)
    {
        Message? message = await _unitOfWork.Messages.GetAsync(messageId);
        if (message == null)
        {
            _logger.LogWarning($"{nameof(DeleteMessageAsync)} - {ErrorMessageConsts.MessageNotFound} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.MessageNotFound));
        }

        _unitOfWork.Messages.Remove(message);
        await _unitOfWork.SaveChangesAsync();

        return Result.Ok();
    }
}
