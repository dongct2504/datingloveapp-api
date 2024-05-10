using FluentResults;

namespace DatingLoveApp.Business.Common.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string message)
    {
        Message = message;
    }
}
