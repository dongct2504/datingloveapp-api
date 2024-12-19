using CloudinaryDotNet.Actions;
using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos.PictureDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Specifications.PictureSpecifications;

namespace SocialChitChat.Business.Services;

public class PictureService : IPictureService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<PictureService> _logger;
    private readonly IMapper _mapper;

    public PictureService(
        UserManager<AppUser> userManager,
        IFileStorageService fileStorageService,
        IMapper mapper,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        ILogger<PictureService> logger,
        SocialChitChatDbContext dbContext)
    {
        _userManager = userManager;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<PictureDto>> UploadPictureAsync(Guid id, IFormFile imageFile)
    {
        AppUser? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            _logger.LogWarning($"{nameof(UploadPictureAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

        string uploadedImagePublicId = string.Empty;

        try
        {
            UploadResult uploadResult = await _fileStorageService.UploadImageAsync(imageFile,
                UploadPath.UserImageUploadPath + user.UserName);
            if (uploadResult.Error != null)
            {
                string message = uploadResult.Error.Message;
                _logger.LogWarning($"{nameof(UploadPictureAsync)} - {message} - {typeof(PictureService)}");
                throw new Exception(message);
            }

            uploadedImagePublicId = uploadResult.PublicId;

            Picture picture = new Picture
            {
                Id = Guid.NewGuid(),
                AppUserId = user.Id,
                ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId,
                CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow,
                UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
            };

            var spec = new AllPicturesByUserIdSpecification(id);
            if (!(await _unitOfWork.Pictures.GetAllWithSpecAsync(spec, true)).Any())
            {
                picture.IsMain = true;
            }
            user.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;
            _unitOfWork.Pictures.Add(picture);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PictureDto>(picture);
        }
        catch (Exception)
        {
            if (uploadedImagePublicId != string.Empty)
            {
                await _fileStorageService.RemoveImageAsync(uploadedImagePublicId);
            }
            throw;
        }
    }

    public async Task<Result> SetMainPictureAsync(Guid id, Guid pictureId)
    {
        AppUser? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            _logger.LogWarning($"{nameof(SetMainPictureAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

        Picture? picture = await _unitOfWork.Pictures
            .GetWithSpecAsync(new PictureByUserIdAndPictureIdSpecification(id, pictureId));
        if (picture == null)
        {
            _logger.LogWarning($"{nameof(SetMainPictureAsync)} - {ErrorMessageConsts.PictureNotFound} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.PictureNotFound));
        }

        if (picture.IsMain)
        {
            _logger.LogWarning($"{nameof(SetMainPictureAsync)} - {ErrorMessageConsts.AlreadyMainPicture} - {typeof(PictureService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.AlreadyMainPicture));
        }

        Picture? currentMain = await _unitOfWork.Pictures
            .GetWithSpecAsync(new MainPictureByUserIdSpecification(id));
        if (currentMain != null)
        {
            currentMain.IsMain = false;
        }

        picture.IsMain = true;
        picture.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;

        user.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;

        await _unitOfWork.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> RemovePictureAsync(Guid userId, Guid pictureId)
    {
        AppUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            _logger.LogWarning($"{nameof(SetMainPictureAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

        Picture? picture = await _unitOfWork.Pictures.GetWithSpecAsync(new PictureExistInUserSpecification(user.Id, pictureId));
        if (picture == null)
        {
            _logger.LogWarning($"{nameof(RemovePictureAsync)} - {ErrorMessageConsts.PictureNotFound} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.PictureNotFound));
        }

        if (picture.IsMain)
        {
            _logger.LogWarning($"{nameof(RemovePictureAsync)} - {ErrorMessageConsts.NotAllowDeleteMainPicture} - {typeof(PictureService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.NotAllowDeleteMainPicture));
        }

        try
        {
            user.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;
            _unitOfWork.Pictures.Remove(picture);

            DeletionResult deletionResult = await _fileStorageService.RemoveImageAsync(picture.PublicId);
            if (deletionResult.Error != null)
            {
                string message = deletionResult.Error.Message;
                _logger.LogWarning($"{nameof(RemovePictureAsync)} - {message} - {typeof(PictureService)}");
                throw new Exception(message);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
