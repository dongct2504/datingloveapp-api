using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class ConversationRepository : Repository<Conversation>, IConversationRepository
{
    public ConversationRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public void Update(Conversation conversation)
    {
        _dbContext.Update(conversation);
    }
}
