using Asp.Versioning;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChitChat.Business.Dtos.GroupChatDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Extensions;

namespace SocialChitChat.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/group-chats")]
public class GroupChatsController : ApiController
{
    private readonly IGroupChatService _groupChatService;

    public GroupChatsController(IGroupChatService groupChatService)
    {
        _groupChatService = groupChatService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GroupChatDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChatListForUser()
    {
        return Ok(await _groupChatService.GetChatListForUserAsync(User.GetCurrentUserId()));
    }

    [HttpGet("{id:guid}", Name = "GetGroupChat")]
    [ProducesResponseType(typeof(GroupChatDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupChat(Guid id, int pageNumber = 1, int pageSize = 30)
    {
        GetGroupChatParams getGroupChatParams = new GetGroupChatParams
        {
            Id = id,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        Result<GroupChatDetailDto> result = await _groupChatService.GetGroupchatAsync(getGroupChatParams);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost("create-group-chat")]
    [ProducesResponseType(typeof(GroupChatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroupChat(
        [FromBody] CreateGroupChatDto request,
        [FromServices] IValidator<CreateGroupChatDto> validator)
    {
        request.AdminId = User.GetCurrentUserId();
        ValidationResult validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Problem(validationResult.Errors);
        }

        Result<GroupChatDto> result = await _groupChatService.CreateGroupChatAsync(request);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return CreatedAtRoute(
            nameof(GetGroupChat),
            new { id = result.Value.Id },
            result.Value
        );
    }

    [HttpPost("add-user-to-group")]
    [ProducesResponseType(typeof(GroupChatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserToGroup(Guid groupChatId, Guid userId)
    {
        Result result = await _groupChatService.AddUserToGroupAsync(groupChatId, userId);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok();
    }

    [HttpPost("add-multiple-users-to-group")]
    [ProducesResponseType(typeof(GroupChatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMultipleUsersToGroup(Guid groupChatId, List<Guid> userIds)
    {
        Result result = await _groupChatService.AddMultipleUsersToGroupAsync(groupChatId, userIds);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok();
    }

    [HttpPost("remove-user-to-group")]
    [ProducesResponseType(typeof(GroupChatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveUserFromGroup(Guid groupChatId, Guid userId)
    {
        Result result = await _groupChatService.RemoveUserFromGroupAsync(groupChatId, userId);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok();
    }

    [HttpPost("remove-multiple-users-to-group")]
    [ProducesResponseType(typeof(GroupChatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveMultipleUsersFromGroup(Guid groupChatId, List<Guid> userIds)
    {
        Result result = await _groupChatService.RemoveMutipleUsersFromGroupAsync(groupChatId, userIds);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok();
    }
}
