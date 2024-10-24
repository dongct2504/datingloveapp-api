using FluentResults;

namespace SocialChitChat.Business.Common.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string message)
    {
        Message = message;
    }
}
