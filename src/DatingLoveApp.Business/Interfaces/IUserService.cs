using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IUserService
{
    Task<PagedList<LocalUserDto>> GetAllAsync(int page);

    Task<Result<LocalUserDto>> GetByIdAsync(Guid id);

    Task<Result<LocalUserDto>> AddAsync(LocalUserDto userDto);

    Task<Result> UpdateAsync(Guid id, CreateLocalUserDto userDto);

    Task<Result> RemoveAsync(UpdateLocalUserDto userDto);
}
