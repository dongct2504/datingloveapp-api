using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.Business.Dtos.PictureDtos;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace DatingLoveApp.Business.Interfaces;

public interface IUserService
{
    Task<PagedList<LocalUserDto>> GetAllAsync(int page);

    Task<Result<LocalUserDetailDto>> GetByIdAsync(Guid id);

    Task<Result<LocalUserDetailDto>> GetByUsernameAsync(string username);

    Task<Result> UpdateAsync(UpdateLocalUserDto userDto);

    Task<Result<PictureDto>> UploadPictureAsync(Guid id, IFormFile imageFile);

    Task<Result> SetMainPictureAsync(Guid id, Guid pictureId);

    Task<Result> RemovePictureAsync(Guid id, Guid pictureId);

    Task<Result> RemoveAsync(Guid id);
}
