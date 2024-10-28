using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.ConversationDtos;
using SocialChitChat.Business.Dtos.MessageDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.Business.Services;

public class GroupChatService : IGroupChatService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GroupChatService> _logger;
    private readonly IMapper _mapper;

    public GroupChatService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        SocialChitChatDbContext dbContext,
        UserManager<AppUser> userManager,
        IDateTimeProvider dateTimeProvider,
        ILogger<GroupChatService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dbContext = dbContext;
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<GroupChatDto>> GetChatListForUserAsync(Guid userId)
    {
        IEnumerable<GroupChat> groupChats = await _dbContext.GroupChats
            .AsNoTracking()
            .Include(c => c.Participants)
                .ThenInclude(c => c.AppUser)
            .Include(c => c.Messages.OrderByDescending(m => m.MessageSent).Take(1))
            .Where(u => u.Participants.Any(p => p.AppUserId == userId))
            .ToListAsync();

        return groupChats
            .Select(c => new GroupChatDto
            {
                Id = c.Id,
                ParticipantIds = c.Participants.Select(p => p.AppUserId).ToList(),
                GroupName = c.GetGroupName(),
                IsGroupChat = c.IsGroupChat,
                LastMessageContent = c.Messages.FirstOrDefault()?.Content ?? string.Empty,
                LastMessageSent = c.Messages.FirstOrDefault()?.MessageSent
            });
    }

    public async Task<Result<GroupChatDetailDto>> GetGroupchatAsync(GetGroupChatParams groupChatParams)
    {
        GroupChat? groupChat = await _dbContext.GroupChats
            .AsNoTracking()
            .Include(c => c.Participants)
            .Where(c => c.Id == groupChatParams.Id && c.IsGroupChat)
            .FirstOrDefaultAsync();

        if (groupChat == null)
        {
            _logger.LogWarning($"{nameof(GetGroupchatAsync)} - {ErrorMessageConsts.ConversationNotFound} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.ConversationNotFound));
        }

        int totalItems = await _dbContext.Messages
            .Where(m => m.ConversationId == groupChat.Id)
            .CountAsync();

        List<MessageDto> messageDtos = await _dbContext.Messages
            .AsNoTracking()
            .Where(m => m.ConversationId == groupChat.Id)
            .OrderByDescending(m => m.MessageSent) // get the latest messages first to paging
            .Skip((groupChatParams.PageNumber - 1) * groupChatParams.PageSize)
            .Take(groupChatParams.PageSize)
            .ProjectToType<MessageDto>()
            .ToListAsync();

        PagedList<MessageDto> pagedList = new PagedList<MessageDto>
        {
            TotalRecords = totalItems,
            Items = messageDtos,
            PageNumber = groupChatParams.PageNumber,
            PageSize = groupChatParams.PageSize
        };

        return new GroupChatDetailDto
        {
            Id = groupChat.Id,
            GroupName = groupChat.GroupName ?? "Unnamed group chat",
            IsGroupChat = groupChat.IsGroupChat,
            ParticipantIds = groupChat.Participants.Select(p => p.AppUserId).ToList(),
            PagedListMessageDto = pagedList
        };
    }

    public async Task<Result<GroupChatDto>> CreateGroupChatAsync(CreateGroupChatDto request)
    {
        GroupChat newGroupChat = new GroupChat
        {
            Id = Guid.NewGuid(),
            GroupName = request.GroupName,
            IsGroupChat = true,
            Participants = new List<Participant>(),
            CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
        };

        if (!await _userManager.Users.AsNoTracking().AnyAsync(u => u.Id == request.AdminId))
        {
            _logger.LogWarning($"{nameof(CreateGroupChatAsync)} - {ErrorMessageConsts.AdminNotFound} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.AdminNotFound));
        }

        // Add the admin to the group participants
        newGroupChat.Participants.Add(new Participant
        {
            AppUserId = request.AdminId,
            IsAdmin = true,
            ConversationId = newGroupChat.Id,
            JoinAt = _dateTimeProvider.LocalVietnamDateTimeNow
        });

        List<Guid> distinctParticipantIds = request.ParticipantIds
            .Distinct()
            .Where(id => id != request.AdminId)
            .ToList(); // 1, 3, 4
        List<Guid> existingUserIds = await _userManager.Users // 1, 2, 4, 5
            .AsNoTracking()
            .Where(u => distinctParticipantIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync(); // 1, 4

        foreach (Guid participantId in distinctParticipantIds)
        {
            if (!existingUserIds.Contains(participantId))
            {
                _logger.LogWarning($"{nameof(CreateGroupChatAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(MessageService)}");
                return Result.Fail(new BadRequestError(ErrorMessageConsts.UserNotFound));
            }

            newGroupChat.Participants.Add(new Participant
            {
                AppUserId = participantId,
                IsAdmin = false,
                ConversationId = newGroupChat.Id,
                JoinAt = _dateTimeProvider.LocalVietnamDateTimeNow
            });
        }

        _unitOfWork.GroupChats.Add(newGroupChat);
        await _unitOfWork.SaveChangesAsync();

        return new GroupChatDto
        {
            Id = newGroupChat.Id,
            ParticipantIds = newGroupChat.Participants.Select(p => p.AppUserId).ToList(),
            GroupName = newGroupChat.GroupName,
            IsGroupChat = newGroupChat.IsGroupChat,
            LastMessageContent = string.Empty,
            LastMessageSent = null
        };
    }

    public Task<Result> AddUserToGroupAsync(Guid conversationId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveUserFromGroupAsync(Guid conversationId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveUsersFromGroupAsync(Guid conversationId, IEnumerable<Guid> userIds)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteConversationForUserAsync(Guid userId, Guid conversationId)
    {
        throw new NotImplementedException();
    }
}
