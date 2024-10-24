using DatingLoveApp.Business.Dtos.PictureDtos;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace DatingLoveApp.Business.Interfaces;

public interface IPictureService
{
    Task<Result<PictureDto>> UploadPictureAsync(string id, IFormFile imageFile);

    Task<Result> SetMainPictureAsync(string id, Guid pictureId);

    Task<Result> RemovePictureAsync(string id, Guid pictureId);
}
