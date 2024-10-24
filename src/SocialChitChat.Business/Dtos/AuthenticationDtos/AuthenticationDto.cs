using SocialChitChat.Business.Dtos.AppUsers;

namespace SocialChitChat.Business.Dtos.AuthenticationDtos;

public class AuthenticationDto
{
    public AppUserDto AppUserDto { get; set; } = null!;

    public string Token { get; set; } = null!;
}
