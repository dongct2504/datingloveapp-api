namespace DatingLoveApp.Business.Dtos.MessageDtos;

public class CreateMessageDto
{
    public string UserId { get; set; } = string.Empty;

    public string RecipientId { get; set; } = null!;

    public string Content { get; set; } = null!;
}
