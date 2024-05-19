using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IUserService
{
    Task<PagedList<LocalUserDto>> GetAllAsync(int page);

    Task<Result<LocalUserDetailDto>> GetByIdAsync(Guid id);

    Task<Result<LocalUserDetailDto>> GetByUsernameAsync(string username);

    Task<Result> UpdateAsync(UpdateLocalUserDto userDto);

    Task<Result> RemoveAsync(Guid id);
}
