using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.ParticipantSpecifications;

public class ParticipantsByGroupChatIdAndUserIdsSpecification : Specification<Participant>
{
    public ParticipantsByGroupChatIdAndUserIdsSpecification(Guid groupChatId, IEnumerable<Guid> userIds)
        : base(p => p.GroupChatId == groupChatId && userIds.Contains(p.AppUserId))
    {
    }
}
