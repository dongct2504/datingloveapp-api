namespace DatingLoveApp.Business.Dtos.PictureDtos;

public class PictureDto
{
    public Guid PictureId { get; set; }

    public string AppUserId { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public bool IsMain { get; set; }

    public string PublicId { get; set; } = null!;
}
