using Asp.Versioning;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AppUsers;
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
[Route("api/v{v:apiVersion}/users")]
public class UsersController : ApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<AppUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] UserParams userParams)
    {
        string id = User.GetCurrentUserId();

        return Ok(await _userService.GetAllAsync(id, userParams));
    }

    [HttpGet("{id}", Name = "GetById")]
    [ProducesResponseType(typeof(AppUserDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
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
    public async Task<IActionResult> GetCurrentUser()
    {
        string id = User.GetCurrentUserId();

        Result<AppUserDto> result = await _userService.GetCurrentUserAsync(id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        string id,
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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(string id)
    {
        Result result = await _userService.RemoveAsync(id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return NoContent();
    }
}
