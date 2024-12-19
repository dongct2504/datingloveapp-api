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
[Route("api/v{v:apiVersion}/likes")]
public class FollowController : ApiController
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpGet("get-follow")]
    [ProducesResponseType(typeof(PagedList<FollowerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFollow([FromQuery] FollowParams followParams)
    {
        followParams.UserId = User.GetCurrentUserId();

        Result<PagedList<FollowerDto>> getUserLikesResult = await _followService
            .GetFollowAsync(followParams);

        if (getUserLikesResult.IsFailed)
        {
            return Problem(getUserLikesResult.Errors);
        }

        return Ok(getUserLikesResult.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserLike(Guid id)
    {
        return Ok(await _followService.IsUserLikedAsync(User.GetCurrentUserId(), id));
    }

    [HttpPost("{id:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLike(Guid id)
    {
        Guid sourceUserId = User.GetCurrentUserId();

        Result<bool> result = await _followService.UpdateLikeAsync(sourceUserId, id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}
