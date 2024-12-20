using Asp.Versioning;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.Business.Dtos.FollowDtos;

namespace SocialChitChat.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/follow")]
public class FollowController : ApiController
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<FollowDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllFollows(string predicate)
    {
        Result<List<FollowDto>> result = await _followService.GetAllFollowsAsync(User.GetCurrentUserId(), predicate);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("get-follows")]
    [ProducesResponseType(typeof(PagedList<FollowDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFollows([FromQuery] FollowParams followParams)
    {
        followParams.UserId = User.GetCurrentUserId();

        Result<PagedList<FollowDto>> result = await _followService.GetFollowsAsync(followParams);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> IsUserFollow(Guid id)
    {
        return Ok(await _followService.IsUserFollowAsync(User.GetCurrentUserId(), id));
    }

    [HttpPost("{id:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateFollow(Guid id)
    {
        Guid sourceUserId = User.GetCurrentUserId();

        Result<bool> result = await _followService.UpdateFollowAsync(sourceUserId, id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}
