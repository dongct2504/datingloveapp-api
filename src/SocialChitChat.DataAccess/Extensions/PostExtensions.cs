using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Extensions;

public static class PostExtensions
{
    public static List<string>? GetPostPictureUrls(this Post post)
    {
        return post.Pictures.Select(p => p.ImageUrl).ToList();
    }
}
