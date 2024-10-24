using Asp.Versioning;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUserLikes;
using SocialChitChat.Business.Interfaces;

namespace SocialChitChat.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/likes")]
public class AppUserLikesController : ApiController
{
    private readonly IAppUserLikeService _appUserLikeService;

    public AppUserLikesController(IAppUserLikeService appUserLikeService)
    {
        _appUserLikeService = appUserLikeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<LikeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUserLikes(string predicate)
    {
        Result<List<LikeDto>> getAllUserLikeResult = await _appUserLikeService
            .GetAllUserLikesAsync(User.GetCurrentUserId(), predicate);

        if (getAllUserLikeResult.IsFailed)
        {
            return Problem(getAllUserLikeResult.Errors);
        }

        return Ok(getAllUserLikeResult.Value);
    }

    [HttpGet("getUserLikes")]
    [ProducesResponseType(typeof(PagedList<LikeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserLikes([FromQuery] AppUserLikeParams likeParams)
    {
        likeParams.UserId = User.GetCurrentUserId();

        Result<PagedList<LikeDto>> getUserLikesResult = await _appUserLikeService
            .GetUserLikesAsync(likeParams);

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
        return Ok(await _appUserLikeService.IsUserLikedAsync(User.GetCurrentUserId(), id));
    }

    [HttpPost("{id:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLike(Guid id)
    {
        Guid sourceUserId = User.GetCurrentUserId();

        Result<bool> result = await _appUserLikeService.UpdateLikeAsync(sourceUserId, id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}
