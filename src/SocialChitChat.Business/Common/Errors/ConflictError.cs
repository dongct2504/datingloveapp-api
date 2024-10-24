using FluentResults;

namespace SocialChitChat.Business.Common.Errors;

public class ConflictError : Error
{
    public ConflictError(string message)
    {
        Message = message;
    }
}
