using Microsoft.EntityFrameworkCore;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.DataAccess.Repositories;

public class ParticipantRepository : Repository<Participant>, IParticipantRepository
{
    public ParticipantRepository(SocialChitChatDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Participant?> GetAsync(Guid groupChatId, Guid userId)
    {
        return await _dbContext.Participants
            .Where(p => p.GroupChatId == groupChatId && p.AppUserId == userId)
            .FirstOrDefaultAsync();
    }

    public void Update(Participant participant)
    {
        _dbContext.Update(participant);
    }
}
