using DatingLoveApp.Business.Dtos.LocalUserDtos;

namespace DatingLoveApp.Business.Dtos.AuthenticationDtos;

public class AuthenticationDto
{
    public LocalUserDto LocalUserDto { get; set; } = null!;

    public string Token { get; set; } = null!;
}
