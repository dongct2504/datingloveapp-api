using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.MessageDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IMessageService
{
    Task<Result<PagedList<MessageDto>>> GetMessagesForUserAsync(MessageParams messageParams);
    Task<Result<PagedList<MessageDto>>> GetMessagesBetweenParticipantsAsync(GetMessageBetweenParticipantsParams participantsParams);

    Task<Result<MessageDto>> SendMessageToRecipientAsync(SendMessageDto createMessageDto);
    Task<Result<MessageDto>> ChangeToReadAsync(Guid id);

    Task<Result> DeleteMessageAsync(Guid messageId);
}
