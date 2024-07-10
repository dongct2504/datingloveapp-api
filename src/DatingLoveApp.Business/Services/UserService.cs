using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AppUsers;
using DatingLoveApp.Business.Dtos.PictureDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Identity;
using DatingLoveApp.DataAccess.Interfaces;
using DatingLoveApp.DataAccess.Specifications.PictureSpecifications;
using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class UserService : IUserService
{
    private readonly DatingLoveAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IPictureRepository _pictureRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public UserService(
        IFileStorageService fileStorageService,
        IMapper mapper,
        UserManager<AppUser> userManager,
        DatingLoveAppDbContext dbContext,
        IPictureRepository pictureRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _userManager = userManager;
        _dbContext = dbContext;
        _pictureRepository = pictureRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PagedList<AppUserDto>> GetAllAsync(string id, UserParams userParams)
    {
        IQueryable<AppUser> query = _userManager.Users.AsQueryable();

        query = query.Where(u => u.Id != id);

        if (userParams.Gender != GenderConstants.Unknown)
        {
            query = query.Where(u => u.Gender == userParams.Gender);
        }

        int minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1).Year; // 30 => 1993
        int maxDob = DateTime.Today.AddYears(-userParams.MinAge).Year; // 20 => 2004

        query = query.Where(u => u.DateOfBirth.Year >= minDob && u.DateOfBirth.Year <= maxDob);

        switch (userParams.SortBy)
        {
            case UserSortConstants.LastActive:
                query = query.OrderByDescending(u => u.LastActive);
                break;

            case UserSortConstants.Created:
                query = query.OrderByDescending(u => u.CreatedAt);
                break;
        }

        int totalRecords = await query.CountAsync();

        List<AppUserDto> users = await query
            .AsNoTracking()
            .Skip((userParams.PageNumber - 1) * userParams.PageSize)
            .Take(userParams.PageSize)
            .ProjectToType<AppUserDto>()
            .ToListAsync();

        string[] userIds = users
            .Select(u => u.Id)
            .ToArray();

        var spec = new MainPicturesByUserIdsSpecification(userIds);
        IEnumerable<Picture> mainPicturesForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec, true);

        foreach (AppUserDto user in users)
        {
            user.ProfilePictureUrl = mainPicturesForEachUser
                .FirstOrDefault(p => p.AppUserId == user.Id)?.ImageUrl;
        }

        PagedList<AppUserDto> pagedList = new PagedList<AppUserDto>
        {
            PageNumber = userParams.PageNumber,
            PageSize = userParams.PageSize,
            TotalRecords = totalRecords,
            Items = users
        };

        return pagedList;
    }

    public async Task<Result<AppUserDetailDto>> GetByIdAsync(string id)
    {
        AppUserDetailDto? userDetailDto = await _userManager.Users
            .Where(u => u.Id == id)
            .AsNoTracking()
            .ProjectToType<AppUserDetailDto>()
            .FirstOrDefaultAsync();

        if (userDetailDto == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        var mainSpec = new MainPictureByUserIdSpecification(id);
        userDetailDto.ProfilePictureUrl = (await _pictureRepository.GetWithSpecAsync(mainSpec, true))?.ImageUrl;

        var spec = new AllPicturesByUserIdSpecification(id);
        userDetailDto.Pictures = _mapper.Map<List<PictureDto>>(
            await _pictureRepository.GetAllWithSpecAsync(spec, true));

        return userDetailDto;
    }

    public async Task<Result<AppUserDetailDto>> GetByUsernameAsync(string username)
    {
        AppUserDetailDto? userDetailDto = await _userManager.Users
            .Where(u => u.UserName == username)
            .AsNoTracking()
            .ProjectToType<AppUserDetailDto>()
            .FirstOrDefaultAsync();

        if (userDetailDto == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        var mainSpec = new MainPictureByUserIdSpecification(userDetailDto.Id);
        userDetailDto.ProfilePictureUrl = (await _pictureRepository.GetWithSpecAsync(mainSpec, true))?.ImageUrl;

        var spec = new AllPicturesByUserIdSpecification(userDetailDto.Id);
        userDetailDto.Pictures = _mapper.Map<List<PictureDto>>(
            await _pictureRepository.GetAllWithSpecAsync(spec, true));

        return userDetailDto;
    }

    public async Task<Result<AppUserDto>> GetCurrentUserAsync(string id)
    {
        AppUserDto? userDto = await _userManager.Users
            .Where(u => u.Id == id)
            .AsNoTracking()
            .ProjectToType<AppUserDto>()
            .FirstOrDefaultAsync();

        if (userDto == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetCurrentUserAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        var mainSpec = new MainPictureByUserIdSpecification(id);
        userDto.ProfilePictureUrl = (await _pictureRepository.GetWithSpecAsync(mainSpec, true))?.ImageUrl;

        return userDto;
    }

    public async Task<Result<List<AppUserDto>>> SearchAsync(string name, string id)
    {
        if (string.IsNullOrEmpty(name))
        {
            string message = "Search term cannot be empty.";
            Log.Warning($"{nameof(SearchAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new BadRequestError(message));
        }

        IQueryable<AppUser> query = _userManager.Users.AsQueryable();

        query = query.Where(u => u.Id != id);

        query = query.Where(u => u.UserName.Contains(name));

        List<AppUserDto> users = await query
            .AsNoTracking()
            .ProjectToType<AppUserDto>()
            .ToListAsync();

        string[] userIds = users
            .Select(u => u.Id)
            .ToArray();

        var spec = new MainPicturesByUserIdsSpecification(userIds);
        IEnumerable<Picture> mainPictureForEachUser = await _pictureRepository.GetAllWithSpecAsync(spec, true);

        foreach (AppUserDto userDto in users)
        {
            userDto.ProfilePictureUrl = mainPictureForEachUser
                .FirstOrDefault(p => p.AppUserId == userDto.Id)?.ImageUrl;
        }

        return users;
    }

    public async Task<Result> UpdateAsync(UpdateAppUserDto updateUserDto)
    {
        AppUser user = await _userManager.FindByIdAsync(updateUserDto.Id);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        _mapper.Map(updateUserDto, user);

        if (!string.IsNullOrEmpty(updateUserDto.Role))
        {
            await _userManager.AddToRoleAsync(user, updateUserDto.Role);
        }

        user.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;

        await _userManager.UpdateAsync(user);

        return Result.Ok();
    }

    public async Task<Result> RemoveAsync(string id)
    {
        AppUser? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(RemoveAsync)} - {message} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(message));
        }

        var spec = new AllPicturesByUserIdSpecification(id);
        IEnumerable<Picture> pictures = await _pictureRepository.GetAllWithSpecAsync(spec, true);

        IEnumerable<Task> tasks = pictures
            .Select(p => _fileStorageService.RemoveImageAsync(p.PublicId));
        await Task.WhenAll(tasks);

        await _userManager.DeleteAsync(user);

        return Result.Ok();
    }
}
