using DatingLoveApp.Business.Common;
using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Extensions;
using DatingLoveApp.DataAccess.Interfaces;
using FluentResults;
using MapsterMapper;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class UserService : IUserService
{
    private readonly ICacheService _cacheService;
    private readonly IUserRepository _userRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IFileStorageService fileStorageService,
        IMapper mapper,
        ICacheService cacheService)
    {
        _userRepository = userRepository;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<PagedList<LocalUserDto>> GetAllAsync(int page)
    {
        string key = "users";

        PagedList<LocalUserDto>? pagedListCache = await _cacheService.GetAsync<PagedList<LocalUserDto>>(key);
        if (pagedListCache != null)
        {
            return pagedListCache;
        }

        QueryOptions<LocalUser> options = new QueryOptions<LocalUser>
        {
            PageNumber = page,
            PageSize = PageSizeConstants.Default50
        };

        PagedList<LocalUserDto> pagedList = new PagedList<LocalUserDto>
        {
            PageNumber = page,
            PageSize = PageSizeConstants.Default50,
            Items = _mapper.Map<IEnumerable<LocalUserDto>>(
                await _userRepository.GetAllAsync(options, asNoTracking: true)),
            TotalRecords = await _userRepository.GetCountAsync()
        };

        await _cacheService.SetAsync(key, pagedList, CacheOptions.DefaultExpiration);

        return pagedList;
    }

    public async Task<Result<LocalUserDto>> GetByIdAsync(Guid id)
    {
        string key = $"user-{id}";

        LocalUserDto? userDtoCache = await _cacheService.GetAsync<LocalUserDto>(key);
        if (userDtoCache != null)
        {
            return userDtoCache;
        }

        LocalUser? user = await _userRepository.GetAsync(new QueryOptions<LocalUser>
        {
            Where = lc => lc.LocalUserId == id
        }, asNoTracking: true);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        userDtoCache = _mapper.Map<LocalUserDto>(user);

        await _cacheService.SetAsync(key, userDtoCache, CacheOptions.DefaultExpiration);

        return userDtoCache;
    }

    public async Task<Result> UpdateAsync(UpdateLocalUserDto userDto)
    {
        LocalUser? user = await _userRepository.GetAsync(new QueryOptions<LocalUser>
        {
            Where = lc => lc.LocalUserId == userDto.LocalUserId
        });
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        _mapper.Map(userDto, user);
        user.Role ??= RoleConstants.Customer;

        if (userDto.ImageFile != null)
        {
            await _fileStorageService.RemoveImageAsync(user.ImageUrl);

            string image = await _fileStorageService.UploadImageAsync(userDto.ImageFile, UploadPath.UserImageUploadPath);
            user.ImageUrl = UploadPath.UserImageUploadPath + image;
        }

        user.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(user);

        await _cacheService.RemoveAsync($"user-{user.LocalUserId}");

        return Result.Ok();
    }

    public async Task<Result> RemoveAsync(Guid id)
    {
        LocalUser? user = await _userRepository.GetAsync(new QueryOptions<LocalUser>
        {
            Where = u => u.LocalUserId == id
        });
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        await _userRepository.RemoveAsync(user);

        await _fileStorageService.RemoveImageAsync(user.ImageUrl);

        await _cacheService.RemoveAsync($"user-{user.LocalUserId}");

        return Result.Ok();
    }
}
