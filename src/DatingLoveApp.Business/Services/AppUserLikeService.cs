using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos.AppUserLikes;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Identity;
using DatingLoveApp.DataAccess.Interfaces;
using DatingLoveApp.DataAccess.Specifications.PictureSpecifications;
using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DatingLoveApp.Business.Services;

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

    public async Task<Result<IEnumerable<LikeDto>>> GetUserLikesAsync(string predicate, string userId)
    {
        IQueryable<AppUser> users = _userManager.Users.AsQueryable();
        IQueryable<AppUserLike> likes = _dbContext.AppUserLikes.AsQueryable();

        IEnumerable<Picture> mainPicturesForEachUser = Enumerable.Empty<Picture>();

        if (predicate == "liked") // get people that the user liked
        {
            string[] userLikedIds = likes
                .Where(like => like.AppUserSourceId == userId)
                .Select(like => like.AppUserLikedId)
                .ToArray();

            users = users.Where(u => userLikedIds.Contains(u.Id));

            var spec = new MainPicturesByUserIdsSpecification(userLikedIds);
            mainPicturesForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec);
        }
        else if (predicate == "likedBy") // get people that liked the user
        {
            string[] likedByOrdersIds = likes
                .Where(like => like.AppUserLikedId == userId)
                .Select(like => like.AppUserSourceId)
                .ToArray();

            users = users.Where(u => likedByOrdersIds.Contains(u.Id));

            var spec = new MainPicturesByUserIdsSpecification(likedByOrdersIds);
            mainPicturesForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec);
        }
        else
        {
            string message = "Predicate is not valid.";
            Log.Warning($"{nameof(GetUserLikesAsync)} - {message} - {typeof(AppUserLikeService)}");
            return Result.Fail(new BadRequestError(message));
        }

        List<LikeDto> likeDtos = await users.Select(u => new LikeDto
        {
            UserId = u.Id,
            UserName = u.UserName,
            Nickname = u.Nickname,
            DateOfBirth = u.DateOfBirth,
            City = u.City
        }).ToListAsync();

        foreach (LikeDto user in likeDtos)
        {
            user.ProfilePictureUrl = mainPicturesForEachUser
                .FirstOrDefault(p => p.AppUserId == user.UserId)?.ImageUrl;
        }

        return likeDtos;
    }

    public async Task<Result> UpdateLikeAsync(string sourceUserId, string likedUserId)
    {
        AppUser? sourceUser = await _userManager.FindByIdAsync(sourceUserId);
        if (sourceUser == null)
        {
            string message = "Source user not found.";
            Log.Warning($"{nameof(UpdateLikeAsync)} - {message} - {typeof(AppUserLikeService)}");
            return Result.Fail(new NotFoundError(message));
        }

        AppUser? likedUser = await _userManager.FindByIdAsync(likedUserId);
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

        AppUserLike? userLike = await _appUserLikeRepository.GetUserLike(sourceUserId, likedUserId);
        if (userLike != null)
        {
            await _appUserLikeRepository.RemoveAsync(userLike);
            return Result.Ok();
        }

        AppUserLike appUserLike = new AppUserLike
        {
            AppUserSourceId = sourceUserId,
            AppUserLikedId = likedUserId,
            CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow,
            UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow
        };

        await _appUserLikeRepository.AddAsync(appUserLike);

        return Result.Ok();
    }
}
