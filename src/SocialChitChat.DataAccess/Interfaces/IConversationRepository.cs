using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IConversationRepository : IRepository<Conversation>
{
    void Update(Conversation conversation);
}
