using DatingLoveApp.Business.Interfaces;

namespace DatingLoveApp.Api.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public FileStorageService(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public async Task<string> UploadImageAsync(IFormFile? image, string uploadPath)
    {
        if (image == null)
        {
            return string.Empty;
        }

        string webRootPath = _hostEnvironment.WebRootPath;

        string path = $"{webRootPath}{TrimWwwroot(uploadPath)}";
        string fileName = Guid.NewGuid().ToString();
        string extension = Path.GetExtension(image.FileName);

        using (FileStream fs = new FileStream($"{path}{fileName}{extension}", FileMode.Create))
        {
            await image.CopyToAsync(fs);
        }

        return fileName + extension;
    }

    public async Task RemoveImageAsync(string? imageUrl)
    {
        if (imageUrl == null)
        {
            return;
        }

        string webRootPath = _hostEnvironment.WebRootPath;

        string oldImagePath = Path.Combine(webRootPath, TrimWwwroot(imageUrl).TrimStart('/'));
        if (File.Exists(oldImagePath))
        {
            await Task.Run(() => File.Delete(oldImagePath));
        }
    }

    private static string TrimWwwroot(string path)
    {
        return path.Substring("/wwwroot".Length);
    }
}
