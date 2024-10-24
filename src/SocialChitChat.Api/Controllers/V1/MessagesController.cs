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
        messageParams.Id = User.GetCurrentUserId();
        PagedList<MessageDto> pagedList = await _messageService.GetMessagesForUserAsync(messageParams);
        return Ok(pagedList);
    }

    [HttpGet("thread/{id}")]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessageThread(string id)
    {
        string userId = User.GetCurrentUserId();
        List<MessageDto> messageDtos = await _messageService.GetMessageThreadAsync(userId, id);
        return Ok(messageDtos);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMessage(
        [FromBody] CreateMessageDto createMessageDto,
        [FromServices] IValidator<CreateMessageDto> validator)
    {
        createMessageDto.UserId = User.GetCurrentUserId();

        ValidationResult validationResult = await validator.ValidateAsync(createMessageDto);
        if (!validationResult.IsValid)
        {
            return Problem(validationResult.Errors);
        }

        Result<MessageDto> result = await _messageService.CreateMessageAsync(createMessageDto);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
