using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Extensions;

public static class LocalUserExtensions
{
    public static string? GetMainProfilePictureUrl(this IEnumerable<Picture> pictures)
    {
        return pictures.FirstOrDefault(p => p.IsMain)?.ImageUrl;
    }
}
