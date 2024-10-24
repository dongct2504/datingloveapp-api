using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AppUsers;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IUserService
{
    Task<PagedList<AppUserDto>> GetAllAsync(string id, UserParams userParams);

    Task<Result<AppUserDetailDto>> GetByIdAsync(string id);

    Task<Result<AppUserDetailDto>> GetByUsernameAsync(string username);

    Task<Result<AppUserDto>> GetCurrentUserAsync(string id);

    Task<Result<List<AppUserDto>>> SearchAsync(string name, string id);

    Task<Result> UpdateAsync(UpdateAppUserDto userDto);

    Task<Result> RemoveAsync(string id);
}
