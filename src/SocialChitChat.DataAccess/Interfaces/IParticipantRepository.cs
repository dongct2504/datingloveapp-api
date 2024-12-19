using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IParticipantRepository : IRepository<Participant>
{
    Task<Participant?> GetAsync(Guid groupChatId, Guid userId);
    void Update(Participant participant);
}
