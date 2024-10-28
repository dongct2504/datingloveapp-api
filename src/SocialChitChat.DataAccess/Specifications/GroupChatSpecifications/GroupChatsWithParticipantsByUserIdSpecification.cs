using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.GroupChatSpecifications;

public class GroupChatsWithParticipantsByUserIdSpecification : Specification<GroupChat>
{
    public GroupChatsWithParticipantsByUserIdSpecification(Guid userId)
        : base(c => c.Participants.Any(p => p.AppUserId == userId))
    {
        AddIncludes(c => c.Participants);
    }
}
