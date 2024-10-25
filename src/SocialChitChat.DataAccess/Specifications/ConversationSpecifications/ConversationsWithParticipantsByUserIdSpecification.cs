using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Specifications.ConversationSpecifications;

public class ConversationsWithParticipantsByUserIdSpecification : Specification<Conversation>
{
    public ConversationsWithParticipantsByUserIdSpecification(Guid userId)
        : base(c => c.Participants.Any(p => p.AppUserId == userId))
    {
        AddIncludes(c => c.Participants);
    }
}
