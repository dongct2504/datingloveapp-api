using Asp.Versioning;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AppUserLikes;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers.V1;

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

    [HttpPost("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLike(string id)
    {
        string sourceUserId = User.GetCurrentUserId();

        Result<bool> result = await _appUserLikeService.UpdateLikeAsync(sourceUserId, id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}
