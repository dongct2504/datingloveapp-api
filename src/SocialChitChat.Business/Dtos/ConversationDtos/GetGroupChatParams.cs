namespace SocialChitChat.Business.Dtos.ConversationDtos;

public class GetGroupChatParams
{
    public Guid Id { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public override string ToString()
    {
        return $"{Id}-{PageNumber}-{PageSize}";
    }
}
