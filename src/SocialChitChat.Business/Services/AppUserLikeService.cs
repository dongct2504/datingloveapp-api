using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUserLikes;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Specifications.PictureSpecifications;

namespace SocialChitChat.Business.Services;

public class AppUserLikeService : IAppUserLikeService
{
    private readonly DatingLoveAppDbContext _dbContext;
    private readonly IAppUserLikeRepository _appUserLikeRepository;
    private readonly IPictureRepository _pictureRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public AppUserLikeService(
        DatingLoveAppDbContext dbContext,
        UserManager<AppUser> userManager,
        IPictureRepository pictureRepository,
        IAppUserLikeRepository appUserLikeRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _pictureRepository = pictureRepository;
        _appUserLikeRepository = appUserLikeRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<List<LikeDto>>> GetAllUserLikesAsync(string userId, string predicate)
    {
        IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
        IQueryable<AppUserLike> likesQuery = _dbContext.AppUserLikes.AsQueryable();

        IEnumerable<Picture> mainPicturesForEachUser = Enumerable.Empty<Picture>();

        if (predicate == "liked") // get people that the user liked
        {
            string[] userLikedIds = likesQuery
                .Where(like => like.AppUserSourceId == userId)
                .Select(like => like.AppUserLikedId)
                .ToArray();

            usersQuery = usersQuery.Where(u => userLikedIds.Contains(u.Id));

            var spec = new MainPicturesByUserIdsSpecification(userLikedIds);
            mainPicturesForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec);
        }
        else if (predicate == "likedBy") // get people that liked the user
        {
            string[] likedByOrdersIds = likesQuery
                .Where(like => like.AppUserLikedId == userId)
                .Select(like => like.AppUserSourceId)
                .ToArray();

            usersQuery = usersQuery
                .Where(u => likedByOrdersIds.Contains(u.Id));

            var spec = new MainPicturesByUserIdsSpecification(likedByOrdersIds);
            mainPicturesForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec);
        }
        else
        {
            string message = "Predicate is not valid.";
            Log.Warning($"{nameof(GetUserLikesAsync)} - {message} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(message));
        }

        List<LikeDto> likeDtos = await usersQuery
            .Select(u => new LikeDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                Nickname = u.Nickname,
                DateOfBirth = u.DateOfBirth,
                City = u.City
            })
            .ToListAsync();

        foreach (LikeDto user in likeDtos)
        {
            user.ProfilePictureUrl = mainPicturesForEachUser
                .FirstOrDefault(p => p.AppUserId == user.UserId)?.ImageUrl;
        }

        return likeDtos;
    }

    public async Task<Result<PagedList<LikeDto>>> GetUserLikesAsync(AppUserLikeParams likeParams)
    {
        IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
        IQueryable<AppUserLike> likesQuery = _dbContext.AppUserLikes.AsQueryable();

        IEnumerable<Picture> mainPicturesForEachUser = Enumerable.Empty<Picture>();

        if (likeParams.Predicate == "liked") // get people that the user liked
        {
            string[] userLikedIds = likesQuery
                .Where(like => like.AppUserSourceId == likeParams.UserId)
                .Select(like => like.AppUserLikedId)
                .ToArray();

            usersQuery = usersQuery.Where(u => userLikedIds.Contains(u.Id));

            var spec = new MainPicturesByUserIdsSpecification(userLikedIds);
            mainPicturesForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec);
        }
        else if (likeParams.Predicate == "likedBy") // get people that liked the user
        {
            string[] likedByOrdersIds = likesQuery
                .Where(like => like.AppUserLikedId == likeParams.UserId)
                .Select(like => like.AppUserSourceId)
                .ToArray();

            usersQuery = usersQuery
                .Where(u => likedByOrdersIds.Contains(u.Id));

            var spec = new MainPicturesByUserIdsSpecification(likedByOrdersIds);
            mainPicturesForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec);
        }
        else
        {
            string message = "Predicate is not valid.";
            Log.Warning($"{nameof(GetUserLikesAsync)} - {message} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(message));
        }

        int totalRecords = await usersQuery.CountAsync();

        List<LikeDto> likeDtos = await usersQuery
            .Select(u => new LikeDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                Nickname = u.Nickname,
                DateOfBirth = u.DateOfBirth,
                City = u.City
            })
            .Skip((likeParams.PageNumber - 1) * likeParams.PageSize)
            .Take(likeParams.PageSize)
            .ToListAsync();

        foreach (LikeDto user in likeDtos)
        {
            user.ProfilePictureUrl = mainPicturesForEachUser
                .FirstOrDefault(p => p.AppUserId == user.UserId)?.ImageUrl;
        }

        PagedList<LikeDto> pagedList = new PagedList<LikeDto>
        {
            PageNumber = likeParams.PageNumber,
            PageSize = likeParams.PageSize,
            TotalRecords = totalRecords,
            Items = likeDtos
        };

        return pagedList;
    }

    public async Task<bool> IsUserLikedAsync(string userSourceId, string userLikedId)
    {
        AppUserLike? userLike = await _appUserLikeRepository.GetUserLike(userSourceId, userLikedId);
        if (userLike == null)
        {
            return false;
        }
        return true;
    }

    public async Task<Result<bool>> UpdateLikeAsync(string userSourceId, string userLikedId)
    {
        AppUser? sourceUser = await _userManager.FindByIdAsync(userSourceId);
        if (sourceUser == null)
        {
            string message = "Source user not found.";
            Log.Warning($"{nameof(UpdateLikeAsync)} - {message} - {typeof(AppUserLikeService)}");
            return Result.Fail(new NotFoundError(message));
        }

        AppUser? likedUser = await _userManager.FindByIdAsync(userLikedId);
        if (likedUser == null)
        {
            string message = "Liked user not found.";
            Log.Warning($"{nameof(UpdateLikeAsync)} - {message} - {typeof(AppUserLikeService)}");
            return Result.Fail(new NotFoundError(message));
        }

        if (sourceUser.UserName == likedUser.UserName)
        {
            string message = "You can not like yourself.";
            Log.Warning($"{nameof(UpdateLikeAsync)} - {message} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(message));
        }

        AppUserLike? userLike = await _appUserLikeRepository.GetUserLike(userSourceId, userLikedId);
        if (userLike != null)
        {
            await _appUserLikeRepository.RemoveAsync(userLike);
            return false;
        }

        AppUserLike appUserLike = new AppUserLike
        {
            AppUserSourceId = userSourceId,
            AppUserLikedId = userLikedId,
            CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow,
            UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
        };

        await _appUserLikeRepository.AddAsync(appUserLike);

        return true;
    }
}
