using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.GroupChatDtos;
using SocialChitChat.Business.Dtos.MessageDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IGroupChatService
{
    Task<IEnumerable<GroupChatDto>> GetChatListForUserAsync(Guid userId);
    Task<Result<PagedList<MessageDto>>> GetGroupchatAsync(GetGroupChatParams groupChatParams);

    Task<Result<GroupChatDto>> CreateGroupChatAsync(CreateGroupChatDto request);
    Task<Result> AddUserToGroupAsync(Guid groupChatId, Guid userId);
    Task<Result> AddMultipleUsersToGroupAsync(Guid groupChatId, List<Guid> userId);

    Task<Result> RemoveUserFromGroupAsync(Guid groupChatId, Guid userId);
    Task<Result> RemoveMutipleUsersFromGroupAsync(Guid groupChatId, List<Guid> userIds);
    Task<Result> DeleteGroupChatForUserAsync(Guid groupChatId, Guid userId);
}
