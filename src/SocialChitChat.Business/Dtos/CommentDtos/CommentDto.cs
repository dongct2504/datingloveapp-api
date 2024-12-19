namespace SocialChitChat.Business.Dtos.CommentDtos;

public class CommentDto
{
    public Guid Id { get; set; }
    public Guid? ReplyId { get; set; }
    public Guid AppUserId { get; set; }
    public string SenderNickName { get; set; } = null!;
    public string? SenderImageUrl { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; } = null!;
}
