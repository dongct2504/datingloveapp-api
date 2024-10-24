using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task UpdateAsync(Message message);
}
