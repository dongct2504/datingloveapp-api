using DatingLoveApp.Business.Common;
using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Data;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Interfaces;
using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class UserService : IUserService
{
    private readonly ICacheService _cacheService;
    private readonly DatingLoveAppDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IFileStorageService fileStorageService,
        IMapper mapper,
        ICacheService cacheService,
        DatingLoveAppDbContext dbContext)
    {
        _userRepository = userRepository;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _cacheService = cacheService;
        _dbContext = dbContext;
    }

    public async Task<PagedList<LocalUserDto>> GetAllAsync(int page)
    {
        string key = $"{CacheConstants.Users}-{page}";

        PagedList<LocalUserDto>? pagedListCache = await _cacheService.GetAsync<PagedList<LocalUserDto>>(key);
        if (pagedListCache != null)
        {
            return pagedListCache;
        }

        IEnumerable<LocalUserDto> localUserDtos = await _dbContext.LocalUsers
            .AsNoTracking()
            .ProjectToType<LocalUserDto>()
            .Skip(PageSizeConstants.Default50 * (page - 1))
            .Take(PageSizeConstants.Default50)
            .ToListAsync();

        PagedList<LocalUserDto> pagedList = new PagedList<LocalUserDto>
        {
            PageNumber = page,
            PageSize = PageSizeConstants.Default50,
            Items = localUserDtos,
            TotalRecords = await _userRepository.GetCountAsync()
        };

        await _cacheService.SetAsync(key, pagedList, CacheOptions.DefaultExpiration);

        return pagedList;
    }

    public async Task<Result<LocalUserDetailDto>> GetByIdAsync(Guid id)
    {
        string key = $"{CacheConstants.User}-{id}";

        LocalUserDetailDto? userDtoCache = await _cacheService.GetAsync<LocalUserDetailDto>(key);
        if (userDtoCache != null)
        {
            return userDtoCache;
        }

        LocalUserDetailDto? userDetailDto = await _dbContext.LocalUsers
            .Where(u => u.LocalUserId == id)
            .AsNoTracking()
            .ProjectToType<LocalUserDetailDto>()
            .SingleOrDefaultAsync();

        if (userDetailDto == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        await _cacheService.SetAsync(key, userDetailDto, CacheOptions.DefaultExpiration);

        return userDetailDto;
    }

    public async Task<Result<LocalUserDetailDto>> GetByUsernameAsync(string username)
    {
        string key = $"{CacheConstants.User}-{username}";

        LocalUserDetailDto? userDtoCache = await _cacheService.GetAsync<LocalUserDetailDto>(key);
        if (userDtoCache != null)
        {
            return userDtoCache;
        }

        LocalUserDetailDto? userDetailDto = await _dbContext.LocalUsers
            .Where(u => u.UserName == username)
            .AsNoTracking()
            .ProjectToType<LocalUserDetailDto>()
            .SingleOrDefaultAsync();

        if (userDetailDto == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        await _cacheService.SetAsync(key, userDetailDto, CacheOptions.DefaultExpiration);

        return userDetailDto;
    }

    public async Task<Result> UpdateAsync(UpdateLocalUserDto userDto)
    {
        LocalUser? user = await _userRepository.GetAsync(userDto.LocalUserId);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        _mapper.Map(userDto, user);
        user.Role ??= RoleConstants.User;
        user.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(user);

        await _cacheService.RemoveAsync($"{CacheConstants.User}-{user.LocalUserId}");

        return Result.Ok();
    }

    public async Task<Result> RemoveAsync(Guid id)
    {
        LocalUser? user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        await _userRepository.RemoveAsync(user);

        await _cacheService.RemoveAsync($"{CacheConstants.User}-{user.LocalUserId}");

        return Result.Ok();
    }
}
