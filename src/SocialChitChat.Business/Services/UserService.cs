using CloudinaryDotNet.Actions;
using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Common;
using SocialChitChat.DataAccess.Common.Enums;
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

    public async Task<PagedList<AppUserDto>> SearchAsync(Guid id, UserParams userParams)
    {
        AppUser? user = await _userManager.Users
            .Include(u => u.AppUserRoles)
                .ThenInclude(ur => ur.AppRole)
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            return new PagedList<AppUserDto>();
        }

        IQueryable<AppUser> query = _userManager.Users
            .Include(u => u.Pictures)
            .AsQueryable();

        query = query.Where(u => u.Id != user.Id);

        if (!string.IsNullOrEmpty(userParams.Name))
        {
            string lowerName = userParams.Name.ToLower();
            query = query.Where(u => u.Nickname.ToLower().Contains(lowerName) || u.UserName.ToLower().Contains(lowerName));
        }

        string[] userRoles = user.AppUserRoles.Select(ur => ur.AppRole.Name).ToArray();
        string[] excludeRoles = Array.Empty<string>();
        if (userRoles.Contains(RoleConstants.Employee) && !userRoles.Contains(RoleConstants.Admin))
        {
            excludeRoles = new string[] { RoleConstants.Admin };
        }
        else if (userRoles.Contains(RoleConstants.User) && !userRoles.Contains(RoleConstants.Employee))
        {
            excludeRoles = new string[] { RoleConstants.Admin, RoleConstants.Employee };
        }

        if (excludeRoles.Any())
        {
            query = query.Where(u => !u.AppUserRoles.Any(ur => excludeRoles.Contains(ur.AppRole.Name)));
        }

        if (userParams.Gender != GenderEnum.Unknown)
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

    public async Task<Result> UpdateAsync(UpdateAppUserDto updateUserDto)
    {
        AppUser user = await _userManager.FindByIdAsync(updateUserDto.Id.ToString());
        if (user == null)
        {
            Log.Warning($"{nameof(GetByIdAsync)} - {ErrorMessageConsts.UserNotFound} - {typeof(UserService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.UserNotFound));
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
            return Result.Fail(new BadRequestError(ErrorMessageConsts.UserNotFound));
        }

        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                string errorMessages = string.Empty;

                IdentityResult deleteUserResult = await _userManager.DeleteAsync(user);
                if (!deleteUserResult.Succeeded)
                {
                    errorMessages = string.Join(", ", deleteUserResult.Errors.Select(e => e.Description));
                    throw new Exception(errorMessages);
                }

                var spec = new AllPicturesByUserIdSpecification(id);
                IEnumerable<Picture> pictures = await _unitOfWork.Pictures.GetAllWithSpecAsync(spec, true);

                Task<DeletionResult>[] deletionTasks = pictures
                    .Select(p => _fileStorageService.RemoveImageAsync(p.PublicId))
                    .ToArray();
                DeletionResult[] deletionResults = await Task.WhenAll(deletionTasks);

                if (deletionResults.Any(result => result.Error != null))
                {
                    errorMessages = string.Join(", ", deletionResults.Select(r => r.Error.Message));
                    throw new Exception(errorMessages);
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Warning($"{nameof(RemoveAsync)} - {ex.Message} - {typeof(PictureService)}");
                return Result.Fail(new BadRequestError(ex.Message));
            }
        };

        return Result.Ok();
    }
}
