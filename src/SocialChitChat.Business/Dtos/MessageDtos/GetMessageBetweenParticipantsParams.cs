namespace SocialChitChat.Business.Dtos.MessageDtos;

public class GetMessageBetweenParticipantsParams
{
    public Guid CurrentUserId { get; set; }
    public Guid RecipientId { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public override string ToString()
    {
        return $"{CurrentUserId}-{RecipientId}-{PageNumber}-{PageSize}";
    }
}
