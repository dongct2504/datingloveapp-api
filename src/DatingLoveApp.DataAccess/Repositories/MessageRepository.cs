using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Interfaces;

namespace DatingLoveApp.DataAccess.Repositories;

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
