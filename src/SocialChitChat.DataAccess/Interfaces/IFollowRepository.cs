using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IFollowRepository : IRepository<Follow>
{
    Task<Follow?> GetUserLike(Guid sorceUserId, Guid likedUserId);

    void Update(Follow appUserLike);
}
