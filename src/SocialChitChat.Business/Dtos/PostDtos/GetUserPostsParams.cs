namespace SocialChitChat.Business.Dtos.PostDtos;

public class GetUserPostsParams : DefaultParams
{
    public Guid UserId { get; set; }
    public int Year { get; set; }
    public int? Month { get; set; }

    public override string? ToString()
    {
        return $"{UserId}-{Year}-{Month}-{base.ToString()}";
    }
}
