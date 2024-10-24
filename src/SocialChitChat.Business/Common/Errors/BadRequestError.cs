using FluentResults;

namespace SocialChitChat.Business.Common.Errors;

public class BadRequestError : Error
{
    public BadRequestError(string message)
    {
        Message = message;
    }
}
