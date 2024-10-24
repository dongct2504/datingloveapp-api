using FluentResults;
using Microsoft.AspNetCore.Http;
using SocialChitChat.Business.Dtos.PictureDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IPictureService
{
    Task<Result<PictureDto>> UploadPictureAsync(string id, IFormFile imageFile);

    Task<Result> SetMainPictureAsync(string id, Guid pictureId);

    Task<Result> RemovePictureAsync(string id, Guid pictureId);
}
