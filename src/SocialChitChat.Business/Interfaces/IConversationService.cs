using FluentResults;
using SocialChitChat.Business.Dtos.ConversationDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IConversationService
{
    Task<IEnumerable<ConversationDto>> GetConversationsForUserAsync(Guid userId);

    Task<Result<ConversationDto>> CreateGroupChatAsync(Guid ownerId, string groupName, IEnumerable<Guid> participantIds);
    Task<Result> AddUserToGroupAsync(Guid conversationId, Guid userId);

    Task<Result> RemoveUserFromGroupAsync(Guid conversationId, Guid userId);
    Task<Result> RemoveUsersFromGroupAsync(Guid conversationId, IEnumerable<Guid> userIds);
    Task<Result> DeleteConversationForUserAsync(Guid userId, Guid conversationId);
}
