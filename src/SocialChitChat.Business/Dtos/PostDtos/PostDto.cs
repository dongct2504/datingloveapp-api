namespace SocialChitChat.Business.Dtos.PostDtos;

public class PostDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string SenderNickName { get; set; } = null!;
    public string? SenderImageUrl { get; set; }
    public string Content { get; set; } = null!;
    public List<string>? PictureUrls { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
