namespace SocialChitChat.Business.Dtos.FollowDtos;

public class FollowParams : DefaultParams
{
    public Guid UserId { get; set; }
    public string Predicate { get; set; } = string.Empty;

    public override string? ToString()
    {
        return $"{UserId}-{Predicate}-{PageNumber}-{PageSize}";
    }
}
