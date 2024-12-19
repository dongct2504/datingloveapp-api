using CloudinaryDotNet.Actions;
using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.PostDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Specifications.PostSpecifications;

namespace SocialChitChat.Business.Services;

public class PostService : IPostService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<PictureService> _logger;
    private readonly IMapper _mapper;

    public PostService(IUnitOfWork unitOfWork,
                       UserManager<AppUser> userManager,
                       IFileStorageService fileStorageService,
                       IDateTimeProvider dateTimeProvider,
                       ILogger<PictureService> logger,
                       IMapper mapper,
                       SocialChitChatDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _fileStorageService = fileStorageService;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<PagedList<PostDto>> GetUserPostsAsync(GetUserPostsParams postsParams)
    {
        IQueryable<Post> postsQuery = _dbContext.Posts.AsQueryable();

        postsQuery = postsQuery.Where(p => p.UserId == postsParams.UserId);
        postsQuery = postsQuery.Where(p => p.CreatedAt.HasValue && p.CreatedAt.Value.Year == postsParams.Year);
        if (postsParams.Month.HasValue)
        {
            postsQuery = postsQuery.Where(p => p.CreatedAt.HasValue && p.CreatedAt.Value.Month == postsParams.Month);
        }

        int totalItems = await postsQuery.CountAsync();

        List<PostDto> postDtos = await postsQuery
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Skip((postsParams.PageNumber - 1) * postsParams.PageSize)
            .Take(postsParams.PageSize)
            .ProjectToType<PostDto>()
            .ToListAsync();

        PagedList<PostDto> pagedList = new PagedList<PostDto>
        {
            TotalRecords = totalItems,
            Items = postDtos,
            PageNumber = postsParams.PageNumber,
            PageSize = postsParams.PageSize
        };

        return pagedList;
    }

    public async Task<Result<PostDetailDto>> GetPostAsync(Guid id)
    {
        PostDetailDto? postDetailDto = await _dbContext.Posts
            .Where(p => p.Id == id)
            .ProjectToType<PostDetailDto>()
            .FirstOrDefaultAsync();
        if (postDetailDto == null)
        {
            _logger.LogWarning($"{nameof(GetPostAsync)} - {ErrorMessageConsts.PostNotFound} - {typeof(PostService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.PostNotFound));
        }

        return postDetailDto;
    }

    public async Task<Result<PostDto>> CreatePostAsync(CreatePostDto dto)
    {
        AppUser? sender = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.Pictures)
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);
        if (sender == null)
        {
            _logger.LogWarning($"{nameof(CreatePostAsync)} - {ErrorMessageConsts.SenderNotFound} - {typeof(PostService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.SenderNotFound));
        }

        if (dto.ImageFiles != null && dto.ImageFiles.Count > 10)
        {
            _logger.LogWarning($"{nameof(CreatePostAsync)} - {ErrorMessageConsts.TooManyPostPictures} - {typeof(PostService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.TooManyPostPictures));
        }

        List<Picture> pictures = new List<Picture>();
        List<string> uploadedImagePublicIds = new List<string>();

        try
        {
            if (dto.ImageFiles != null && dto.ImageFiles.Any())
            {
                var uploadTasks = dto.ImageFiles.Select(async image =>
                {
                    UploadResult uploadResult = await _fileStorageService.UploadImageAsync(image, UploadPath.PostImageUploadPath + sender.UserName);

                    if (uploadResult.Error != null)
                    {
                        string message = uploadResult.Error.Message;
                        _logger.LogWarning($"{nameof(CreatePostAsync)} - {message} - {typeof(PostService)}");
                        throw new Exception(message);
                    }

                    uploadedImagePublicIds.Add(uploadResult.PublicId);

                    return new Picture
                    {
                        Id = Guid.NewGuid(),
                        AppUserId = sender.Id,
                        ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow,
                        UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
                    };
                });

                pictures.AddRange(await Task.WhenAll(uploadTasks));
            }

            if (pictures.Any())
            {
                _unitOfWork.Pictures.AddRange(pictures);
            }

            Post post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = sender.Id,
                Content = dto.Content,
                CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow,
                Pictures = pictures
            };
            _unitOfWork.Posts.Add(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }
        catch (Exception)
        {
            if (uploadedImagePublicIds.Any())
            {
                var deletionTasks = uploadedImagePublicIds.Select(uploadedImagePublicId =>
                    _fileStorageService.RemoveImageAsync(uploadedImagePublicId));
                await Task.WhenAll(deletionTasks);
            }
            throw;
        }
    }

    public async Task<Result> UpdatePostAsync(UpdatePostDto dto)
    {
        AppUser? sender = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.Pictures)
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);
        if (sender == null)
        {
            _logger.LogWarning($"{nameof(CreatePostAsync)} - {ErrorMessageConsts.SenderNotFound} - {typeof(PostService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.SenderNotFound));
        }

        Post? post = await _unitOfWork.Posts.GetWithSpecAsync(new PostWithPicturesByIdSpecification(dto.Id));
        if (post == null)
        {
            _logger.LogWarning($"{nameof(CreatePostAsync)} - {ErrorMessageConsts.PostNotFound} - {typeof(PostService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.PostNotFound));
        }

        if (dto.ImageFiles != null && dto.ImageFiles.Count > 10)
        {
            _logger.LogWarning($"{nameof(CreatePostAsync)} - {ErrorMessageConsts.TooManyPostPictures} - {typeof(PostService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.TooManyPostPictures));
        }

        List<Picture> pictures = new List<Picture>();
        List<string> uploadedImagePublicIds = new List<string>();

        try
        {
            if (dto.ImageFiles != null && dto.ImageFiles.Any())
            {
                List<Picture> pictureOld = post.Pictures.ToList();

                var uploadTasks = dto.ImageFiles.Select(async image =>
                {
                    UploadResult uploadResult = await _fileStorageService.UploadImageAsync(image, UploadPath.PostImageUploadPath + sender.UserName);

                    if (uploadResult.Error != null)
                    {
                        string message = uploadResult.Error.Message;
                        _logger.LogWarning($"{nameof(CreatePostAsync)} - {message} - {typeof(PostService)}");
                        throw new Exception(message);
                    }

                    uploadedImagePublicIds.Add(uploadResult.PublicId);

                    return new Picture
                    {
                        Id = Guid.NewGuid(),
                        AppUserId = sender.Id,
                        ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                        CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow,
                        UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
                    };
                });

                pictures.AddRange(await Task.WhenAll(uploadTasks));

                if (pictureOld.Any())
                {
                    _unitOfWork.Pictures.RemoveRange(pictureOld);

                    var deletionTasks = pictureOld.Select(pictureOld =>
                        _fileStorageService.RemoveImageAsync(pictureOld.PublicId));
                    await Task.WhenAll(deletionTasks);
                }
            }

            if (pictures.Any())
            {
                _unitOfWork.Pictures.AddRange(pictures);
            }

            post.Content = dto.Content;
            post.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;
            post.Pictures = pictures;

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception)
        {
            if (uploadedImagePublicIds.Any())
            {
                var deletionTasks = uploadedImagePublicIds.Select(uploadedImagePublicId =>
                    _fileStorageService.RemoveImageAsync(uploadedImagePublicId));
                await Task.WhenAll(deletionTasks);
            }
            throw;
        }
    }

    public Task<Result> RemoveAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
