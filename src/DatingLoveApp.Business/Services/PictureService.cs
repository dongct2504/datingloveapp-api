using CloudinaryDotNet.Actions;
using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos.PictureDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Identity;
using DatingLoveApp.DataAccess.Interfaces;
using DatingLoveApp.DataAccess.Specifications.PictureSpecifications;
using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class PictureService : IPictureService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPictureRepository _pictureRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public PictureService(
        UserManager<AppUser> userManager,
        IPictureRepository pictureRepository,
        IFileStorageService fileStorageService,
        IMapper mapper)
    {
        _userManager = userManager;
        _pictureRepository = pictureRepository;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
    }

    public async Task<Result<PictureDto>> UploadPictureAsync(string id, IFormFile imageFile)
    {
        AppUser? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(UploadPictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(message));
        }

        UploadResult uploadResult = await _fileStorageService.UploadImageAsync(imageFile,
            UploadPath.UserImageUploadPath + user.UserName);
        if (uploadResult.Error != null)
        {
            string message = uploadResult.Error.Message;
            Log.Warning($"{nameof(UploadPictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new BadRequestError(message));
        }

        Picture picture = new Picture()
        {
            PictureId = Guid.NewGuid(),
            AppUserId = user.Id,
            ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
            PublicId = uploadResult.PublicId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        if (await _pictureRepository.GetCountAsync() == 0)
        {
            picture.IsMain = true;
        }

        await _pictureRepository.AddAsync(picture);

        return _mapper.Map<PictureDto>(picture);
    }

    public async Task<Result> SetMainPictureAsync(string id, Guid pictureId)
    {
        AppUser? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(SetMainPictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(message));
        }

        var pictureSpec = new PictureByUserIdSpecification(id);
        Picture? picture = await _pictureRepository.GetWithSpecAsync(pictureSpec, asNoTracking: true);
        if (picture == null)
        {
            string message = "Picture not found.";
            Log.Warning($"{nameof(SetMainPictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(message));
        }

        if (picture.IsMain)
        {
            string message = "This picture is already your main picture.";
            Log.Warning($"{nameof(SetMainPictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new BadRequestError(message));
        }

        var spec = new MainPictureByUserIdSpecification(id);
        Picture? currentMain = await _pictureRepository.GetWithSpecAsync(spec);
        if (currentMain != null)
        {
            currentMain.IsMain = false;
        }

        picture.IsMain = true;

        await _pictureRepository.UpdateAsync(picture);

        return Result.Ok();
    }

    public async Task<Result> RemovePictureAsync(string id, Guid pictureId)
    {
        AppUser? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(SetMainPictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(message));
        }

        Picture? picture = await _pictureRepository.GetAsync(pictureId);
        if (picture == null)
        {
            string message = "Picture not found.";
            Log.Warning($"{nameof(RemovePictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(message));
        }

        if (picture.IsMain && await _pictureRepository.GetCountAsync() > 1)
        {
            string message = "You can't delete your main picture.";
            Log.Warning($"{nameof(RemovePictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new BadRequestError(message));
        }

        DeletionResult deletionResult = await _fileStorageService.RemoveImageAsync(picture.PublicId);
        if (deletionResult.Error != null)
        {
            string message = deletionResult.Error.Message;
            Log.Warning($"{nameof(RemovePictureAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new BadRequestError(message));
        }

        await _pictureRepository.RemoveAsync(picture);

        return Result.Ok();
    }
}
