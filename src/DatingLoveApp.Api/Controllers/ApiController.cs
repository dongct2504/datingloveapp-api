using DatingLoveApp.Business.Common.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<IError> errors)
    {
        IError firstError = errors.First();

        switch (firstError)
        {
            case NotFoundError:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: firstError.Message);
            case BadRequestError:
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: firstError.Message);
            case ConflictError:
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);
            default:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: firstError.Message);
        }
    }
}
