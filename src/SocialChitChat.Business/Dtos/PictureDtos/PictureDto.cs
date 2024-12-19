namespace SocialChitChat.Business.Dtos.PictureDtos;

public class PictureDto
{
    public Guid PictureId { get; set; }
    public Guid AppUserId { get; set; }
    public Guid? PostId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }
    public string PublicId { get; set; } = null!;
}
