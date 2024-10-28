namespace SocialChitChat.Business.Dtos.ConversationDtos;

public class CreateGroupChatDto
{
    public Guid AdminId { get; set; }
    public string GroupName { get; set; } = null!;
    public Guid[] ParticipantIds { get; set; } = Array.Empty<Guid>();
}
