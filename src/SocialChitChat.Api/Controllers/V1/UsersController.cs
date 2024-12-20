using Asp.Versioning;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Extensions;

namespace SocialChitChat.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/users")]
public class UsersController : ApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedList<AppUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] UserParams userParams)
    {
        Guid id = User.GetCurrentUserId();

        return Ok(await _userService.SearchAsync(id, userParams));
    }

    [HttpGet("{id:guid}", Name = "GetById")]
    [ProducesResponseType(typeof(AppUserDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        Result<AppUserDetailDto> result = await _userService.GetByIdAsync(id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpGet("username/{username}")]
    [ProducesResponseType(typeof(AppUserDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUsername(string username)
    {
        Result<AppUserDetailDto> result = await _userService.GetByUsernameAsync(username);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpGet("current-user")]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser()
    {
        Guid id = User.GetCurrentUserId();

        Result<AppUserDto> result = await _userService.GetCurrentUserAsync(id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAppUserDto request,
        [FromServices] IValidator<UpdateAppUserDto> validator)
    {
        if (id != request.Id)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Id not match.");
        }

        ValidationResult validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Problem(validationResult.Errors);
        }

        Result result = await _userService.UpdateAsync(request);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid id)
    {
        Result result = await _userService.RemoveAsync(id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return NoContent();
    }
}
