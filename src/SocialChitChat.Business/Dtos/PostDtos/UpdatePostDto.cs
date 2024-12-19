using Microsoft.AspNetCore.Http;

namespace SocialChitChat.Business.Dtos.PostDtos;

public class UpdatePostDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = null!;
    public List<IFormFile>? ImageFiles { get; set; }
}
