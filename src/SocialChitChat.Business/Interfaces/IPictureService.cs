using FluentResults;
using Microsoft.AspNetCore.Http;
using SocialChitChat.Business.Dtos.PictureDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IPictureService
{
    Task<Result<PictureDto>> UploadPictureAsync(Guid userId, IFormFile imageFile);

    Task<Result> SetMainPictureAsync(Guid userId, Guid pictureId);

    Task<Result> RemovePictureAsync(Guid userId, Guid pictureId);
}
