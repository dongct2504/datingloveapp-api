using Microsoft.AspNetCore.Http;

namespace SocialChitChat.Business.Dtos.PostDtos;

public class CreatePostDto
{
    public Guid UserId { get; set; }
    public string Content { get; set; } = null!;
    public List<IFormFile>? ImageFiles { get; set; }
}
