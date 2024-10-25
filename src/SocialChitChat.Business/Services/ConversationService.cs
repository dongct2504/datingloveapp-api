using FluentResults;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SocialChitChat.Business.Dtos.ConversationDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Specifications.ConversationSpecifications;

namespace SocialChitChat.Business.Services;

public class ConversationService : IConversationService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConversationService(IUnitOfWork unitOfWork, IMapper mapper, SocialChitChatDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ConversationDto>> GetConversationsForUserAsync(Guid userId)
    {
        IEnumerable<Conversation> conversations = await _dbContext.Conversations
            .AsNoTracking()
            .Include(c => c.Participants)
            .ThenInclude(c => c.AppUser)
            .Include(c => c.Messages.OrderByDescending(m => m.MessageSent).Take(1))
            .Where(u => u.Participants.Any(p => p.AppUserId == userId))
            .ToListAsync();

        return conversations
            .Select(c => new ConversationDto
            {
                Id = c.Id,
                ParticipantIds = c.Participants.Select(p => p.AppUserId).ToList(),
                GroupName = c.GroupName ?? c.GetRecipientNickName(userId),
                IsGroupChat = c.IsGroupChat,
                LastMessageContent = c.Messages.First().Content,
                LastMessageSent = c.Messages.First().MessageSent
            });
    }

    public Task<Result<ConversationDto>> CreateGroupChatAsync(Guid adminId, string groupName, IEnumerable<Guid> participantIds)
    {
        throw new NotImplementedException();
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
