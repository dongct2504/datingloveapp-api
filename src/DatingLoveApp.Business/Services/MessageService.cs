using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.MessageDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Identity;
using DatingLoveApp.DataAccess.Interfaces;
using DatingLoveApp.DataAccess.Specifications.PictureSpecifications;
using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class MessageService : IMessageService
{
    private readonly DatingLoveAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMessageRepository _messageRepository;
    private readonly IPictureRepository _pictureRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public MessageService(
        UserManager<AppUser> userManager,
        IDateTimeProvider dateTimeProvider,
        IMessageRepository messageRepository,
        IMapper mapper,
        IPictureRepository pictureRepository,
        DatingLoveAppDbContext dbContext)
    {
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _messageRepository = messageRepository;
        _mapper = mapper;
        _pictureRepository = pictureRepository;
        _dbContext = dbContext;
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
    {
        IQueryable<Message> messagesQuery = _dbContext.Messages
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        switch (messageParams.Contain)
        {
            case MessageConstants.Inbox:
                messagesQuery = messagesQuery
                    .Where(m => m.RecipientId == messageParams.Id && !m.RecipientDeleted);
                break;

            case MessageConstants.Outbox:
                messagesQuery = messagesQuery
                    .Where(m => m.SenderId == messageParams.Id && !m.SenderDeleted);
                break;

            case MessageConstants.Unread:
                messagesQuery = messagesQuery
                    .Where(m => m.RecipientId == messageParams.Id && m.DateRead == null && !m.RecipientDeleted);
                break;
        }

        int totalCount = await messagesQuery.CountAsync();

        List<MessageDto> messageDtos = await messagesQuery
            .AsNoTracking()
            .Skip((messageParams.PageNumber - 1) * messageParams.PageSize)
            .Take(messageParams.PageSize)
            .ProjectToType<MessageDto>()
            .ToListAsync();

        string[] senderIds = messageDtos
            .Select(m => m.SenderId)
            .ToArray();
        string[] recipientIds = messageDtos
            .Select(m => m.RecipientId)
            .ToArray();

        var senderSpec = new MainPicturesByUserIdsSpecification(senderIds);
        IEnumerable<Picture> senderMainProfiles = await _pictureRepository.GetAllWithSpecAsync(senderSpec, true);

        var recipientSpec = new MainPicturesByUserIdsSpecification(recipientIds);
        IEnumerable<Picture> recipientMainProfiles = await _pictureRepository.GetAllWithSpecAsync(recipientSpec, true);

        foreach (MessageDto messageDto in messageDtos)
        {
            messageDto.SenderImageUrl = senderMainProfiles
                .FirstOrDefault(p => p.AppUserId == messageDto.SenderId)?.ImageUrl;
            messageDto.RecipientImageUrl = recipientMainProfiles
                .FirstOrDefault(p => p.AppUserId == messageDto.RecipientId)?.ImageUrl;
        }

        PagedList<MessageDto> pagedList = new PagedList<MessageDto>
        {
            PageNumber = messageParams.PageNumber,
            PageSize = messageParams.PageSize,
            TotalRecords = totalCount,
            Items = messageDtos
        };

        return pagedList;
    }

    public async Task<Result<MessageDto>> ChangeToReadAsync(Guid id)
    {
        Message? messageToChange = await _messageRepository.GetAsync(id);
        if (messageToChange == null)
        {
            string message = "Message not found.";
            Log.Warning($"{nameof(ChangeToReadAsync)} - {message} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(message));
        }

        messageToChange.DateRead = _dateTimeProvider.LocalVietnamDateTimeNow;

        await _dbContext.SaveChangesAsync();

        MessageDto messageDto = _mapper.Map<MessageDto>(messageToChange);

        var spec = new MainPicturesByUserIdsSpecification(
            new string[2] { messageDto.SenderId, messageDto.RecipientId });
        IEnumerable<Picture> pictures = await _pictureRepository.GetAllWithSpecAsync(spec, true);

        messageDto.SenderImageUrl = pictures
            .FirstOrDefault(p => p.AppUserId == messageDto.SenderId)?.ImageUrl;

        messageDto.RecipientImageUrl = pictures
            .FirstOrDefault(p => p.AppUserId == messageDto.RecipientId)?.ImageUrl;

        return messageDto;
    }

    public async Task<Result<MessageDto>> CreateMessageAsync(CreateMessageDto createMessageDto)
    {
        AppUser? user = await _userManager.FindByIdAsync(createMessageDto.UserId);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(CreateMessageAsync)} - {message} - {typeof(MessageService)}");
            return Result.Fail(new BadRequestError(message));
        }

        AppUser? recipient = await _userManager.FindByIdAsync(createMessageDto.RecipientId);
        if (recipient == null)
        {
            string message = "Recipient not found.";
            Log.Warning($"{nameof(CreateMessageAsync)} - {message} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(message));
        }

        if (user.UserName == recipient.UserName)
        {
            string message = "You cannot send messages to yourself";
            Log.Warning($"{nameof(CreateMessageAsync)} - {message} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(message));
        }

        Message messageToRecipient = new Message
        {
            MessageId = Guid.NewGuid(),
            SenderId = user.Id,
            SenderUserName = user.UserName,
            RecipientId = recipient.Id,
            RecipientUserName = recipient.UserName,
            Content = createMessageDto.Content,
            MessageSent = _dateTimeProvider.LocalVietnamDateTimeNow
        };

        await _messageRepository.AddAsync(messageToRecipient);

        MessageDto messageDto = _mapper.Map<MessageDto>(messageToRecipient);

        var spec = new MainPicturesByUserIdsSpecification(
            new string[2] { messageDto.SenderId, messageDto.RecipientId });
        IEnumerable<Picture> pictures = await _pictureRepository.GetAllWithSpecAsync(spec, true);

        messageDto.SenderImageUrl = pictures
            .FirstOrDefault(p => p.AppUserId == messageDto.SenderId)?.ImageUrl;

        messageDto.RecipientImageUrl = pictures
            .FirstOrDefault(p => p.AppUserId == messageDto.RecipientId)?.ImageUrl;

        return messageDto;
    }

    public async Task<List<MessageDto>> GetMessageThreadAsync(string currentUserId, string recipientId)
    {
        List<Message> messages = await _dbContext.Messages
            .Where(m =>
                (m.RecipientId == currentUserId && m.SenderId == recipientId && !m.RecipientDeleted) ||
                (m.RecipientId == recipientId && m.SenderId == currentUserId && !m.SenderDeleted))
            .OrderBy(m => m.MessageSent)
            .ToListAsync();

        List<Message> unreadMessages = messages
            .Where(m => m.DateRead == null && m.RecipientId == currentUserId)
            .ToList();

        if (unreadMessages.Any())
        {
            foreach (Message unreadMessage in unreadMessages)
            {
                unreadMessage.DateRead = _dateTimeProvider.LocalVietnamDateTimeNow;
            }
            await _dbContext.SaveChangesAsync();
        }

        string[] senderIds = messages.Select(m => m.SenderId).ToArray();
        string[] recipientIds = messages.Select(m => m.RecipientId).ToArray();

        var senderSpec = new MainPicturesByUserIdsSpecification(senderIds);
        IEnumerable<Picture> senderMainProfile = await _pictureRepository.GetAllWithSpecAsync(senderSpec, true);

        var recipientSpec = new MainPicturesByUserIdsSpecification(recipientIds);
        IEnumerable<Picture> recipientMainProfile = await _pictureRepository.GetAllWithSpecAsync(recipientSpec, true);

        List<MessageDto> messageDtos = _mapper.Map<List<MessageDto>>(messages);

        foreach (MessageDto messageDto in messageDtos)
        {
            messageDto.SenderImageUrl = senderMainProfile
                .FirstOrDefault(m => m.AppUserId == messageDto.SenderId)?.ImageUrl;
            messageDto.RecipientImageUrl = recipientMainProfile
                .FirstOrDefault(m => m.AppUserId == messageDto.RecipientId)?.ImageUrl;
        }

        return messageDtos;
    }

    public async Task<Result> DeleteMessageAsync(string userId, Guid messageId)
    {
        Message? messageFromDb = await _messageRepository.GetAsync(messageId);
        if (messageFromDb == null)
        {
            string message = "Message not found.";
            Log.Warning($"{nameof(DeleteMessageAsync)} - {message} - {typeof(MessageService)}");
            return Result.Fail(new NotFoundError(message));
        }

        if (messageFromDb.SenderId != userId && messageFromDb.RecipientId != userId)
        {
            string message = "Unauthorize";
            Log.Warning($"{nameof(DeleteMessageAsync)} - {message} - {typeof(MessageService)}");
            return Result.Fail(new UnauthorizeError(message));
        }

        if (messageFromDb.SenderId == userId)
        {
            messageFromDb.SenderDeleted = true;
        }

        if (messageFromDb.RecipientId == userId)
        {
            messageFromDb.RecipientDeleted = true;
        }

        if (messageFromDb.SenderDeleted && messageFromDb.RecipientDeleted)
        {
            await _messageRepository.RemoveAsync(messageFromDb);
        }

        await _messageRepository.SaveAllAsync();

        return Result.Ok();
    }
}
