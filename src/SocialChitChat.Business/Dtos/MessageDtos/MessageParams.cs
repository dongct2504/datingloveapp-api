namespace SocialChitChat.Business.Dtos.MessageDtos;

public class MessageParams
{
    public Guid UserId { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;

    public override string? ToString()
    {
        return $"{UserId}-{PageNumber}-{PageSize}";
    }
}
