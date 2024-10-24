using FluentResults;

namespace DatingLoveApp.Business.Common.Errors;

public class UnauthorizeError : Error
{
    public UnauthorizeError(string message)
    {
        Message = message;
    }
}
