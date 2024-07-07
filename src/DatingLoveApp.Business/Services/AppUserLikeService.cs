using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos;
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
