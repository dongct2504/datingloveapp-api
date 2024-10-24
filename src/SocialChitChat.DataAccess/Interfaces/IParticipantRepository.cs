using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IParticipantRepository : IRepository<Participant>
{
    void Update(Participant participant);
}
