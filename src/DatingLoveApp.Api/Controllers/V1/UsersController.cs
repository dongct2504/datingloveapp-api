using Asp.Versioning;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.Business.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers.V1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/users")]
public class UsersController : ApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<LocalUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int page = 1)
    {
        return Ok(await _userService.GetAllAsync(page));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LocalUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        Result<LocalUserDto> result = await _userService.GetByIdAsync(id);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateLocalUserDto request)
    {
        if (id != request.LocalUserId)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Id not match.");
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
