using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthenticationDto>> RegisterAsync(RegisterAppUserDto userDto);

    Task<Result<AuthenticationDto>> LoginAsync(LoginAppUserDto userDto);
}
