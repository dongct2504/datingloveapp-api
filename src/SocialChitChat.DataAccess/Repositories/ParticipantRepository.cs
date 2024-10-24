using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class ParticipantRepository : Repository<Participant>, IParticipantRepository
{
    public ParticipantRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public void Update(Participant participant)
    {
        _dbContext.Update(participant);
    }
}
