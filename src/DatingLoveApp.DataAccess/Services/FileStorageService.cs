using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingLoveApp.DataAccess.Common;
using DatingLoveApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingLoveApp.DataAccess.Services;

public class FileStorageService : IFileStorageService
{
    private readonly Cloudinary _cloudinary;

    public FileStorageService(IOptions<CloudinarySettings> options)
    {
        Account account = new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile image, string uploadPath)
    {
        ImageUploadResult uploadResult = new ImageUploadResult();

        if (image.Length > 0)
        {
            using (Stream stream = image.OpenReadStream())
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = uploadPath,
                    File = new FileDescription(image.FileName, stream),
                    Transformation = new Transformation()
                        .Height(500)
                        .Width(500)
                        .Crop("fill")
                        .Gravity("face")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
        }

        return uploadResult;
    }

    public async Task<DeletionResult> RemoveImageAsync(string publicId)
    {
        DeletionParams deletionParams = new DeletionParams(publicId);

        DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);

        return deletionResult;
    }
}
