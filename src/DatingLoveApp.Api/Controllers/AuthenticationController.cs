using Asp.Versioning;
using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using DatingLoveApp.Business.Interfaces;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers;

[ApiVersionNeutral]
[Route("api/v{v:apiVersion}/authen")]
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterAppUserDto request,
        [FromServices] IValidator<RegisterAppUserDto> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Problem(validationResult.Errors);
        }

        Result<AuthenticationDto> result = await _authenticationService.RegisterAsync(request);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromBody] LoginAppUserDto request,
        [FromServices] IValidator<LoginAppUserDto> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Problem(validationResult.Errors);
        }

        Result<AuthenticationDto> result = await _authenticationService.LoginAsync(request);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}
