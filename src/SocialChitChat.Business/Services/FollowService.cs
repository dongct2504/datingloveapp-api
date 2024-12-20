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

    public async Task<Result<List<FollowDto>>> GetAllFollowsAsync(Guid userId, string predicate)
    {
        IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
        IQueryable<Follow> followQuery = _dbContext.Follows.AsQueryable();

        IEnumerable<Picture> mainPicturesForEachUser = Enumerable.Empty<Picture>();

        if (predicate == "following") // get people that the user following
        {
            usersQuery = followQuery
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Following);
        }
        else if (predicate == "follower") // get people that follow the user
        {
            usersQuery = followQuery
                .Where(f => f.FollowingId == userId)
                .Select(f => f.Follower);
        }
        else
        {
            _logger.LogWarning($"{nameof(GetFollowsAsync)} - {ErrorMessageConsts.InvalidPredicate} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.InvalidPredicate));
        }

        return await usersQuery
            .Select(u => new FollowDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                Nickname = u.Nickname,
                ProfilePictureUrl = u.GetMainProfilePictureUrl(),
                DateOfBirth = u.DateOfBirth,
                City = u.City
            })
            .ToListAsync();
    }

    public async Task<Result<PagedList<FollowDto>>> GetFollowsAsync(FollowParams followParams)
    {
        IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
        IQueryable<Follow> followQuery = _dbContext.Follows.AsQueryable();

        IEnumerable<Picture> mainPicturesForEachUser = Enumerable.Empty<Picture>();

        if (followParams.Predicate == "following") // get people that the user following
        {
            usersQuery = followQuery
                .Where(f => f.FollowerId == followParams.UserId)
                .Select(f => f.Following);
        }
        else if (followParams.Predicate == "follower") // get people that follow the user
        {
            usersQuery = followQuery
                .Where(f => f.FollowingId == followParams.UserId)
                .Select(f => f.Follower);
        }
        else
        {
            _logger.LogWarning($"{nameof(GetFollowsAsync)} - {ErrorMessageConsts.InvalidPredicate} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.InvalidPredicate));
        }

        int totalRecords = await usersQuery.CountAsync();

        List<FollowDto> followDtos = await usersQuery
            .Select(u => new FollowDto
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

        PagedList<FollowDto> pagedList = new PagedList<FollowDto>
        {
            PageNumber = followParams.PageNumber,
            PageSize = followParams.PageSize,
            TotalRecords = totalRecords,
            Items = followDtos
        };

        return pagedList;
    }

    public async Task<bool> IsUserFollowAsync(Guid userSourceId, Guid userLikedId)
    {
        Follow? follow = await _unitOfWork.Follows.GetUserFollow(userSourceId, userLikedId);
        if (follow == null)
        {
            return false;
        }
        return true;
    }

    public async Task<Result<bool>> UpdateFollowAsync(Guid userSourceId, Guid userLikedId)
    {
        AppUser? sourceUser = await _userManager.FindByIdAsync(userSourceId.ToString());
        if (sourceUser == null)
        {
            _logger.LogWarning($"{nameof(UpdateFollowAsync)} - {ErrorMessageConsts.SourceUserNotFound} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.SourceUserNotFound));
        }

        AppUser? likedUser = await _userManager.FindByIdAsync(userLikedId.ToString());
        if (likedUser == null)
        {
            _logger.LogWarning($"{nameof(UpdateFollowAsync)} - {ErrorMessageConsts.LikedUserNotFound} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.LikedUserNotFound));
        }

        if (sourceUser.Id == likedUser.Id)
        {
            _logger.LogWarning($"{nameof(UpdateFollowAsync)} - {ErrorMessageConsts.LikeYourselfError} - {typeof(FollowService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.LikeYourselfError));
        }

        Follow? follow = await _unitOfWork.Follows.GetUserFollow(userSourceId, userLikedId);
        if (follow != null)
        {
            _unitOfWork.Follows.Remove(follow);
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
