using FluentResults;
using SocialChitChat.Business.Dtos.AuthenticationDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthenticationDto>> RegisterAsync(RegisterAppUserDto userDto);

    Task<Result<AuthenticationDto>> LoginAsync(LoginAppUserDto userDto);
}
