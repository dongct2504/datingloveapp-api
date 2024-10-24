using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(DatingLoveAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task UpdateAsync(Message message)
    {
        _dbContext.Update(message);
        await _dbContext.SaveChangesAsync();
    }
}
