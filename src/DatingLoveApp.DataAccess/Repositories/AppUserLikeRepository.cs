using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Interfaces;

namespace DatingLoveApp.DataAccess.Repositories;

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
