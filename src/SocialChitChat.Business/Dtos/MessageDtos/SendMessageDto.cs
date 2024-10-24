namespace SocialChitChat.Business.Dtos.MessageDtos;

public class SendMessageDto
{
    public Guid SenderId { get; set; }

    public Guid RecipientId { get; set; }

    public string Content { get; set; } = null!;
}
