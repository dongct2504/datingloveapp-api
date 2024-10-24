using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IAppUserLikeRepository : IRepository<AppUserLike>
{
    Task<AppUserLike?> GetUserLike(string sorceUserId, string likedUserId);

    Task UpdateAsync(AppUserLike appUserLike);
}
