using Asp.Versioning;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using SocialChitChat.DataAccess.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.MessageDtos;
using SocialChitChat.Business.Interfaces;

namespace SocialChitChat.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/messages")]
public class MessagesController : ApiController
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<MessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMessageForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.UserId = User.GetCurrentUserId();
        Result<PagedList<MessageDto>> result = await _messageService.GetMessagesForUserAsync(messageParams);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("conversation-between-participants")]
    [ProducesResponseType(typeof(PagedList<MessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessagesBetweenParticipants(
        [FromQuery] GetMessageBetweenParticipantsParams participantsParams)
    {
        participantsParams.CurrentUserId = User.GetCurrentUserId();
        Result<PagedList<MessageDto>> result = await _messageService
            .GetMessagesBetweenParticipantsAsync(participantsParams);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessageToRecipient(
        [FromBody] SendMessageDto sendMessageDto,
        [FromServices] IValidator<SendMessageDto> validator)
    {
        sendMessageDto.SenderId = User.GetCurrentUserId();

        ValidationResult validationResult = await validator.ValidateAsync(sendMessageDto);
        if (!validationResult.IsValid)
        {
            return Problem(validationResult.Errors);
        }

        Result<MessageDto> result = await _messageService.SendMessageToRecipientAsync(sendMessageDto);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost("change-to-read/{id:guid}")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeToRead(Guid id)
    {
        Result<MessageDto> result = await _messageService.ChangeToReadAsync(id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        Result deleteMessageResult = await _messageService.DeleteMessageAsync(User.GetCurrentUserId(), id);
        if (deleteMessageResult.IsFailed)
        {
            return Problem(deleteMessageResult.Errors);
        }

        return NoContent();
    }
}
