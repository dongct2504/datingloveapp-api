using FluentResults;

namespace DatingLoveApp.Business.Common.Errors;

public class BadRequestError : Error
{
    public BadRequestError(string message)
    {
        Message = message;
    }
}
