namespace SocialChitChat.Business.Dtos.MessageDtos;

public class GetMessageBetweenParticipantsParams : DefaultParams
{
    public Guid CurrentUserId { get; set; }
    public Guid RecipientId { get; set; }

    public override string ToString()
    {
        return $"{CurrentUserId}-{RecipientId}-{PageNumber}-{PageSize}";
    }
}
