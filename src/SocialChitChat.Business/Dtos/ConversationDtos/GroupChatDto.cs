namespace SocialChitChat.Business.Dtos.ConversationDtos;

public class GroupChatDto
{
    public Guid Id { get; set; }
    public string GroupName { get; set; } = null!;
    public bool IsGroupChat { get; set; }
    public List<Guid> ParticipantIds { get; set; } = new List<Guid>();
    public string LastMessageContent { get; set; } = string.Empty;
    public DateTime? LastMessageSent { get; set; }
}
