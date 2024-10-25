using SocialChitChat.DataAccess.Entities.AutoGenEntities;

namespace SocialChitChat.DataAccess.Extensions;

public static class ConversationExtensions
{
    public static string GetRecipientNickName(this Conversation conversation, Guid currentUserId)
    {
        Participant? recipient = conversation.Participants
            .FirstOrDefault(p => p.AppUserId != currentUserId);

        return recipient?.AppUser?.Nickname ?? "Unnamed Conversation";
    }
}
