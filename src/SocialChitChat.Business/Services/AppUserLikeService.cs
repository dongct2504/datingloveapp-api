using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUserLikes;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.Business.Services;

public class AppUserLikeService : IAppUserLikeService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AppUserLikeService> _logger;
    private readonly IMapper _mapper;

    public AppUserLikeService(
        SocialChitChatDbContext dbContext,
        UserManager<AppUser> userManager,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<AppUserLikeService> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<LikeDto>>> GetAllUserLikesAsync(Guid userId, string predicate)
    {
        IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
        IQueryable<AppUserLike> likesQuery = _dbContext.AppUserLikes.AsQueryable();

        List<AppUser> user = new List<AppUser>();

        if (predicate == "liked") // get people that the user liked
        {
            usersQuery = likesQuery
                .Where(like => like.AppUserSourceId == userId)
                .Select(like => like.AppUserSource);
        }
        else if (predicate == "likedBy") // get people that liked the user
        {
            usersQuery = likesQuery
                .Where(like => like.AppUserLikedId == userId)
                .Select(like => like.AppUserLiked);
        }
        else
        {
            _logger.LogWarning($"{nameof(GetUserLikesAsync)} - {ErrorMessageConsts.InvalidPredicate} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.InvalidPredicate));
        }

        List<LikeDto> likeDtos = await usersQuery
            .Select(u => new LikeDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                Nickname = u.Nickname,
                ProfilePictureUrl = u.GetMainProfilePictureUrl(),
                DateOfBirth = u.DateOfBirth,
                City = u.City
            })
            .ToListAsync();

        return likeDtos;
    }

    public async Task<Result<PagedList<LikeDto>>> GetUserLikesAsync(AppUserLikeParams likeParams)
    {
        IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
        IQueryable<AppUserLike> likesQuery = _dbContext.AppUserLikes.AsQueryable();

        IEnumerable<Picture> mainPicturesForEachUser = Enumerable.Empty<Picture>();

        if (likeParams.Predicate == "liked") // get people that the user liked
        {
            usersQuery = likesQuery
                .Where(like => like.AppUserSourceId == likeParams.UserId)
                .Select(like => like.AppUserSource);
        }
        else if (likeParams.Predicate == "likedBy") // get people that liked the user
        {
            usersQuery = likesQuery
                .Where(like => like.AppUserLikedId == likeParams.UserId)
                .Select(like => like.AppUserLiked);
        }
        else
        {
            _logger.LogWarning($"{nameof(GetUserLikesAsync)} - {ErrorMessageConsts.InvalidPredicate} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.InvalidPredicate));
        }

        int totalRecords = await usersQuery.CountAsync();

        List<LikeDto> likeDtos = await usersQuery
            .Select(u => new LikeDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                Nickname = u.Nickname,
                ProfilePictureUrl = u.GetMainProfilePictureUrl(),
                DateOfBirth = u.DateOfBirth,
                City = u.City
            })
            .Skip((likeParams.PageNumber - 1) * likeParams.PageSize)
            .Take(likeParams.PageSize)
            .ToListAsync();

        PagedList<LikeDto> pagedList = new PagedList<LikeDto>
        {
            PageNumber = likeParams.PageNumber,
            PageSize = likeParams.PageSize,
            TotalRecords = totalRecords,
            Items = likeDtos
        };

        return pagedList;
    }

    public async Task<bool> IsUserLikedAsync(Guid userSourceId, Guid userLikedId)
    {
        AppUserLike? userLike = await _unitOfWork.AppUserLikes.GetUserLike(userSourceId, userLikedId);
        if (userLike == null)
        {
            return false;
        }
        return true;
    }

    public async Task<Result<bool>> UpdateLikeAsync(Guid userSourceId, Guid userLikedId)
    {
        AppUser? sourceUser = await _userManager.FindByIdAsync(userSourceId.ToString());
        if (sourceUser == null)
        {
            _logger.LogWarning($"{nameof(UpdateLikeAsync)} - {ErrorMessageConsts.SourceUserNotFound} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.SourceUserNotFound));
        }

        AppUser? likedUser = await _userManager.FindByIdAsync(userLikedId.ToString());
        if (likedUser == null)
        {
            _logger.LogWarning($"{nameof(UpdateLikeAsync)} - {ErrorMessageConsts.LikedUserNotFound} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.LikedUserNotFound));
        }

        if (sourceUser.Id == likedUser.Id)
        {
            _logger.LogWarning($"{nameof(UpdateLikeAsync)} - {ErrorMessageConsts.LikeYourselfError} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.LikeYourselfError));
        }

        AppUserLike? userLike = await _unitOfWork.AppUserLikes.GetUserLike(userSourceId, userLikedId);
        if (userLike != null)
        {
            _unitOfWork.AppUserLikes.Remove(userLike);
            await _unitOfWork.SaveChangesAsync();
            return false;
        }

        AppUserLike appUserLike = new AppUserLike
        {
            AppUserSourceId = userSourceId,
            AppUserLikedId = userLikedId,
            CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
        };

        _unitOfWork.AppUserLikes.Add(appUserLike);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
