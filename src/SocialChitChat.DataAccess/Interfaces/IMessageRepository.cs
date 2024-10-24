using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    void Update(Message message);
}
