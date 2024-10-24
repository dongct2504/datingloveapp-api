namespace SocialChitChat.Business.Dtos.AppUserLikes;

public class AppUserLikeParams
{
    public string UserId { get; set; } = string.Empty;

    public string Predicate { get; set; } = string.Empty;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;

    public override string? ToString()
    {
        return $"{UserId}-{Predicate}-{PageNumber}-{PageSize}";
    }
}
