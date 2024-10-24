using SocialChitChat.DataAccess.Identity;
using System.Security.Claims;

namespace SocialChitChat.DataAccess.Extensions;

public static class UserExtensions
{
    public static Guid GetCurrentUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public static string? GetMainProfilePictureUrl(this AppUser user)
    {
        return user.Pictures.FirstOrDefault(u => u.IsMain)?.ImageUrl;
    }
}
