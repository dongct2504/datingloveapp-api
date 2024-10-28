using FluentResults;
using SocialChitChat.Business.Dtos.ConversationDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IGroupChatService
{
    Task<IEnumerable<GroupChatDto>> GetChatListForUserAsync(Guid userId);
    Task<Result<GroupChatDetailDto>> GetGroupchatAsync(GetGroupChatParams groupChatParams);

    Task<Result<GroupChatDto>> CreateGroupChatAsync(CreateGroupChatDto request);
    Task<Result> AddUserToGroupAsync(Guid conversationId, Guid userId);

    Task<Result> RemoveUserFromGroupAsync(Guid conversationId, Guid userId);
    Task<Result> RemoveUsersFromGroupAsync(Guid conversationId, IEnumerable<Guid> userIds);
    Task<Result> DeleteConversationForUserAsync(Guid userId, Guid conversationId);
}
