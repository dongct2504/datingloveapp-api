using CloudinaryDotNet.Actions;
using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Enums;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Entities.AutoGenEntities;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Specifications.PictureSpecifications;

namespace SocialChitChat.Business.Services;

public class UserService : IUserService
{
    private readonly SocialChitChatDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public UserService(
        IFileStorageService fileStorageService,
        IMapper mapper,
        UserManager<AppUser> userManager,
        SocialChitChatDbContext dbContext,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _userManager = userManager;
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<AppUserDto>> GetAllAsync(Guid id, UserParams userParams)
    {
        IQueryable<AppUser> query = _userManager.Users
            .Include(u => u.Pictures)
            .AsQueryable();

        query = query.Where(u => u.Id != id);

        if (userParams.Gender != GenderEnums.Unknown)
        {
            query = query.Where(u => u.Gender == (byte)userParams.Gender);
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

            case UserSortConstants.Nickname:
                query = query.OrderBy(u => u.Nickname);
                break;
        }

        int totalRecords = await query.CountAsync();

        List<AppUserDto> users = await query
            .AsNoTracking()
            .Skip((userParams.PageNumber - 1) * userParams.PageSize)
            .Take(userParams.PageSize)
            .ProjectToType<AppUserDto>()
            .ToListAsync();

        PagedList<AppUserDto> pagedList = new PagedList<AppUserDto>
        {
            PageNumber = userParams.PageNumber,
            PageSize = userParams.PageSize,
            TotalRecords = totalRecords,
            Items = users
        };

        return pagedList;
    }

    public async Task<Result<AppUserDetailDto>> GetByIdAsync(Guid id)
    {
        AppUserDetailDto? userDetailDto = await _userManager.Users
            .Where(u => u.Id == id)
            .AsNoTracking()
            .ProjectToType<AppUserDetailDto>()
            .FirstOrDefaultAsync();

        if (userDetailDto == null)
        {
            Log.Warning($"{nameof(GetByIdAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

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
            Log.Warning($"{nameof(GetByIdAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

        return userDetailDto;
    }

    public async Task<Result<AppUserDto>> GetCurrentUserAsync(Guid id)
    {
        AppUserDto? userDto = await _userManager.Users
            .Where(u => u.Id == id)
            .AsNoTracking()
            .ProjectToType<AppUserDto>()
            .FirstOrDefaultAsync();

        if (userDto == null)
        {
            Log.Warning($"{nameof(GetCurrentUserAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

        return userDto;
    }

    public async Task<Result<List<AppUserDto>>> SearchAsync(string name, Guid id)
    {
        IQueryable<AppUser> query = _userManager.Users.AsQueryable();

        query = query.Where(u => u.Id != id);

        if (!string.IsNullOrEmpty(name))
        {
            string toLowerName = name.ToLower();
            query = query.Where(u => u.UserName.ToLower().Contains(toLowerName) ||
                u.Nickname.ToLower().Contains(toLowerName));
        }

        List<AppUserDto> usersDto = await query
            .AsNoTracking()
            .ProjectToType<AppUserDto>()
            .ToListAsync();

        return usersDto;
    }

    public async Task<Result> UpdateAsync(UpdateAppUserDto updateUserDto)
    {
        AppUser user = await _userManager.FindByIdAsync(updateUserDto.Id.ToString());
        if (user == null)
        {
            Log.Warning($"{nameof(GetByIdAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

        _mapper.Map(updateUserDto, user);

        user.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;

        await _userManager.UpdateAsync(user);

        return Result.Ok();
    }

    public async Task<Result> RemoveAsync(Guid id)
    {
        AppUser? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            Log.Warning($"{nameof(RemoveAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(PictureService)}");
            return Result.Fail(new NotFoundError(ErrorMessageConsts.UserNotFound));
        }

        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                IdentityResult deleteUserResult = await _userManager.DeleteAsync(user);
                if (!deleteUserResult.Succeeded)
                {
                    throw new Exception("User deletion failed.");
                }

                var spec = new AllPicturesByUserIdSpecification(id);
                IEnumerable<Picture> pictures = await _unitOfWork.Pictures.GetAllWithSpecAsync(spec, true);

                Task<DeletionResult>[] deletionTasks = pictures
                    .Select(p => _fileStorageService.RemoveImageAsync(p.PublicId))
                    .ToArray();
                DeletionResult[] deletionResults = await Task.WhenAll(deletionTasks);

                if (deletionResults.Any(result => result.Error != null))
                {
                    throw new Exception("One or more image deletions failed.");
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        };

        return Result.Ok();
    }
}
