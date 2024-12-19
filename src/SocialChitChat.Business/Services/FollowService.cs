using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.FollowDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.Business.Services;

public class FollowService : IFollowService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<FollowService> _logger;
    private readonly IMapper _mapper;

    public FollowService(
        SocialChitChatDbContext dbContext,
        UserManager<AppUser> userManager,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<FollowService> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PagedList<FollowerDto>>> GetFollowAsync(FollowParams followParams)
    {
        IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
        IQueryable<Follow> followQuery = _dbContext.Follows.AsQueryable();

        IEnumerable<Picture> mainPicturesForEachUser = Enumerable.Empty<Picture>();

        if (followParams.Predicate == "following") // get people that the user following
        {
            usersQuery = followQuery
                .Where(f => f.FollowerId == followParams.UserId)
                .Select(f => f.Follower);
        }
        else if (followParams.Predicate == "follower") // get people that follow the user
        {
            usersQuery = followQuery
                .Where(f => f.FollowingId == followParams.UserId)
                .Select(f => f.Following);
        }
        else
        {
            _logger.LogWarning($"{nameof(GetFollowAsync)} - {ErrorMessageConsts.InvalidPredicate} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.InvalidPredicate));
        }

        int totalRecords = await usersQuery.CountAsync();

        List<FollowerDto> likeDtos = await usersQuery
            .Select(u => new FollowerDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                Nickname = u.Nickname,
                ProfilePictureUrl = u.GetMainProfilePictureUrl(),
                DateOfBirth = u.DateOfBirth,
                City = u.City
            })
            .Skip((followParams.PageNumber - 1) * followParams.PageSize)
            .Take(followParams.PageSize)
            .ToListAsync();

        PagedList<FollowerDto> pagedList = new PagedList<FollowerDto>
        {
            PageNumber = followParams.PageNumber,
            PageSize = followParams.PageSize,
            TotalRecords = totalRecords,
            Items = likeDtos
        };

        return pagedList;
    }

    public async Task<bool> IsUserLikedAsync(Guid userSourceId, Guid userLikedId)
    {
        Follow? userLike = await _unitOfWork.Follows.GetUserLike(userSourceId, userLikedId);
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
            _logger.LogWarning($"{nameof(UpdateLikeAsync)} - {ErrorMessageConsts.SourceUserNotFound} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.SourceUserNotFound));
        }

        AppUser? likedUser = await _userManager.FindByIdAsync(userLikedId.ToString());
        if (likedUser == null)
        {
            _logger.LogWarning($"{nameof(UpdateLikeAsync)} - {ErrorMessageConsts.LikedUserNotFound} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.LikedUserNotFound));
        }

        if (sourceUser.Id == likedUser.Id)
        {
            _logger.LogWarning($"{nameof(UpdateLikeAsync)} - {ErrorMessageConsts.LikeYourselfError} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.LikeYourselfError));
        }

        Follow? userLike = await _unitOfWork.Follows.GetUserLike(userSourceId, userLikedId);
        if (userLike != null)
        {
            _unitOfWork.Follows.Remove(userLike);
            await _unitOfWork.SaveChangesAsync();
            return false;
        }

        Follow appUserLike = new Follow
        {
            FollowerId = userSourceId,
            FollowingId = userLikedId,
            CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
        };

        _unitOfWork.Follows.Add(appUserLike);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
