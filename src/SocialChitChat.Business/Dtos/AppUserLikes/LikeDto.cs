namespace SocialChitChat.Business.Dtos.AppUserLikes;

public class LikeDto
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string? City { get; set; }
}
