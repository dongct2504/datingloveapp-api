using FluentResults;

namespace SocialChitChat.Business.Common.Errors;

public class UnauthorizeError : Error
{
    public UnauthorizeError(string message)
    {
        Message = message;
    }
}
