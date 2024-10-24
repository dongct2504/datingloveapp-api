using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.MessageDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IMessageService
{
    Task<Result<MessageDto>> SendMessageToRecipientAsync(SendMessageDto createMessageDto);

    Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);

    Task<Result<MessageDto>> ChangeToReadAsync(Guid id);

    Task<List<MessageDto>> GetMessageThreadAsync(Guid currentUserId, Guid recipientId);

    Task<Result> DeleteMessageAsync(Guid userId, Guid messageId);
}
