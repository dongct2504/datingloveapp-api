namespace SocialChitChat.Business.Dtos.GroupChatDtos;

public class GetGroupChatParams : DefaultParams
{
    public Guid Id { get; set; }

    public override string ToString()
    {
        return $"{Id}-{PageNumber}-{PageSize}";
    }
}
