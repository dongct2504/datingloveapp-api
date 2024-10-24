using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class AppUserLikeRepository : Repository<AppUserLike>, IAppUserLikeRepository
{
    public AppUserLikeRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<AppUserLike?> GetUserLike(Guid sorceUserId, Guid likedUserId)
    {
        return await _dbContext.AppUserLikes.FindAsync(sorceUserId, likedUserId);
    }

    public void Update(AppUserLike appUserLike)
    {
        _dbContext.Update(appUserLike);
    }
}
