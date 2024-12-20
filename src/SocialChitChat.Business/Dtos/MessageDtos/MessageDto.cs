namespace SocialChitChat.Business.Dtos.MessageDtos;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }

    public Guid SenderId { get; set; }
    public string SenderUserName { get; set; } = null!;
    public string SenderNickName { get; set; } = null!;
    public string? SenderImageUrl { get; set; }

    public Guid RecipientId { get; set; }
    public string RecipientUserName { get; set; } = null!;
    public string RecipientNickName { get; set; } = null!;
    public string? RecipientImageUrl { get; set; }

    public string Content { get; set; } = null!;
    public DateTime MessageSent { get; set; }
    public DateTime? DateRead { get; set; }
}
