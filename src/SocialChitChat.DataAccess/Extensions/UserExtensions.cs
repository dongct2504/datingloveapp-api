using System.Security.Claims;

namespace SocialChitChat.DataAccess.Extensions;

public static class UserExtensions
{
    public static string GetCurrentUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}
