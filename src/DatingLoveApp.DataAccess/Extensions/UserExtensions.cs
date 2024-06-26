using System.Security.Claims;

namespace DatingLoveApp.DataAccess.Extensions;

public static class UserExtensions
{
    public static Guid GetCurrentUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }
}
