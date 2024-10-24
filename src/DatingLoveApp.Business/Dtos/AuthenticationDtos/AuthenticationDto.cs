using DatingLoveApp.Business.Dtos.AppUsers;

namespace DatingLoveApp.Business.Dtos.AuthenticationDtos;

public class AuthenticationDto
{
    public AppUserDto AppUserDto { get; set; } = null!;

    public string Token { get; set; } = null!;
}
