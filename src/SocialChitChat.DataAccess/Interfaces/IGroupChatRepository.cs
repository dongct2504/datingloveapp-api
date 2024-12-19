using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IGroupChatRepository : IRepository<GroupChat>
{
    void Update(GroupChat conversation);
}
