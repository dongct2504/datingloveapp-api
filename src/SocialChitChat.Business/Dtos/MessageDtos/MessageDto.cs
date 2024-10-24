namespace SocialChitChat.Business.Dtos.MessageDtos;

public class MessageDto
{
    public Guid MessageId { get; set; }

    public string SenderId { get; set; } = null!;

    public string SenderUserName { get; set; } = null!;

    public string? SenderImageUrl { get; set; }

    public string RecipientId { get; set; } = null!;

    public string RecipientUserName { get; set; } = null!;

    public string? RecipientImageUrl { get; set; }

    public string Content { get; set; } = null!;

    public DateTime MessageSent { get; set; }

    public DateTime? DateRead { get; set; }
}
