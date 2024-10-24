using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.MessageDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IMessageService
{
    Task<Result<MessageDto>> CreateMessageAsync(CreateMessageDto createMessageDto);

    Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);

    Task<Result<MessageDto>> ChangeToReadAsync(Guid id);

    Task<List<MessageDto>> GetMessageThreadAsync(string currentUserId, string recipientId);

    Task<Result> DeleteMessageAsync(string userId, Guid messageId);

    Task AddUserToGroupAsync(string groupName, string userId);

    Task RemoveUserFromGroupAsync(string groupName, string userId);

    Task<List<string>> GetGroupUsersAsync(string groupName);
}
