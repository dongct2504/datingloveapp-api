namespace SocialChitChat.Business.Dtos.ConversationDtos;

public class SendMessageToGroupRequest
{
    public Guid SenderId { get; set; }
    public Guid[] ParticipantIds { get; set; } = Array.Empty<Guid>();
    public string Content { get; set; } = null!;
}
