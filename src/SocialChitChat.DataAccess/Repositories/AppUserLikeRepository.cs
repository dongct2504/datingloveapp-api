using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class AppUserLikeRepository : Repository<AppUserLike>, IAppUserLikeRepository
{
    public AppUserLikeRepository(DatingLoveAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<AppUserLike?> GetUserLike(string sorceUserId, string likedUserId)
    {
        return await _dbContext.AppUserLikes.FindAsync(sorceUserId, likedUserId);
    }

    public async Task UpdateAsync(AppUserLike appUserLike)
    {
        _dbContext.Update(appUserLike);
        await _dbContext.SaveChangesAsync();
    }
}
