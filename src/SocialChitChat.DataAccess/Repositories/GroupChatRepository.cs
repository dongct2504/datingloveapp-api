using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class GroupChatRepository : Repository<GroupChat>, IGroupChatRepository
{
    public GroupChatRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public void Update(GroupChat conversation)
    {
        _dbContext.Update(conversation);
    }
}
