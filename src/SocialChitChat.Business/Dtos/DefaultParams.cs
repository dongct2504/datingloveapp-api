namespace SocialChitChat.Business.Dtos;

public class DefaultParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 30;

    public override string? ToString()
    {
        return $"{PageNumber}-{PageSize}";
    }
}
