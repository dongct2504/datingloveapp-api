using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IFileStorageService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile image, string uploadPath);

    Task<DeletionResult> RemoveImageAsync(string publicId);
}
