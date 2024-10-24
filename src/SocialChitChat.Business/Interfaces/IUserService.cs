using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUsers;

namespace SocialChitChat.Business.Interfaces;

public interface IUserService
{
    Task<PagedList<AppUserDto>> GetAllAsync(Guid id, UserParams userParams);

    Task<Result<AppUserDetailDto>> GetByIdAsync(Guid id);

    Task<Result<AppUserDetailDto>> GetByUsernameAsync(string username);

    Task<Result<AppUserDto>> GetCurrentUserAsync(Guid id);

    Task<Result<List<AppUserDto>>> SearchAsync(string name, Guid id);

    Task<Result> UpdateAsync(UpdateAppUserDto userDto);

    Task<Result> RemoveAsync(Guid id);
}
