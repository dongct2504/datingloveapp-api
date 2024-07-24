using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.MessageDtos;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IMessageService
{
    Task<Result<MessageDto>> CreateMessageAsync(CreateMessageDto createMessageDto);

    Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);

    Task<List<MessageDto>> GetMessageThreadAsync(string currentUserId, string recipientId);

    Task<Result> DeleteMessageAsync(string userId, Guid messageId);
}
