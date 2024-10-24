using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IAppUserLikeRepository : IRepository<AppUserLike>
{
    Task<AppUserLike?> GetUserLike(Guid sorceUserId, Guid likedUserId);

    void Update(AppUserLike appUserLike);
}
