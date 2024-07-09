using Asp.Versioning;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.MessageDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Extensions;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers.V1;

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
}
