using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class FollowRepository : Repository<Follow>, IFollowRepository
{
    public FollowRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Follow?> GetUserLike(Guid sorceUserId, Guid likedUserId)
    {
        return await _dbContext.Follows.FindAsync(sorceUserId, likedUserId);
    }

    public void Update(Follow appUserLike)
    {
        _dbContext.Update(appUserLike);
    }
}
