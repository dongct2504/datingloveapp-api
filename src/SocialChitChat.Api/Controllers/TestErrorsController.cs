﻿using Asp.Versioning;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.DataAccess.Data;

namespace SocialChitChat.Api.Controllers;

[ApiVersionNeutral]
[Route("api/v{v:apiVersion}/test-errors")]
public class TestErrorsController : ApiController
{
    private readonly DatingLoveAppDbContext _dbContext;

    public TestErrorsController(DatingLoveAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("notfound")]
    public IActionResult Test404NotFound()
    {
        return Problem(Result.Fail(new NotFoundError("not found error.")).Errors);
    }

    [HttpGet("badrequest")]
    public IActionResult Test400BadRequest()
    {
        return Problem(Result.Fail(new BadRequestError("bad request error.")).Errors);
    }

    [HttpGet("unauthorize")]
    [Authorize]
    public IActionResult TestAuthError()
    {
        return Ok();
    }

    [HttpGet("validation-errors")]
    public IActionResult TestValidationErrors([FromServices] IValidator<UpdateAppUserDto> validator)
    {
        UpdateAppUserDto updateAppUserDto = new UpdateAppUserDto();
        ValidationResult validationResult = validator.Validate(updateAppUserDto);

        return Problem(validationResult.Errors);
    }

    [HttpGet("internal-server-error")]
    public IActionResult TestInternalServerError()
    {
        var things = _dbContext.Pictures.Find(-1);
        var thingsToReturn = things!.ToString();
        return Ok(thingsToReturn);
    }
}
