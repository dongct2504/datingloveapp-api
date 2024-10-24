using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task UpdateAsync(Message message);
}
