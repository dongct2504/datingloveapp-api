using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public void Update(Message message)
    {
        _dbContext.Update(message);
    }
}
