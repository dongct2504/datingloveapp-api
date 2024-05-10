using Microsoft.AspNetCore.Http;

namespace DatingLoveApp.Business.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadImageAsync(IFormFile? image, string uploadPath);

    Task RemoveImageAsync(string? imageUrl);
}
