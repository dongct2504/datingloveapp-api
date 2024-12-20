using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class FollowRepository : Repository<Follow>, IFollowRepository
{
    public FollowRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Follow?> GetUserFollow(Guid sorceUserId, Guid followedUserId)
    {
        return await _dbContext.Follows.FindAsync(sorceUserId, followedUserId);
    }

    public void Update(Follow appUserFollow)
    {
        _dbContext.Update(appUserFollow);
    }
}
