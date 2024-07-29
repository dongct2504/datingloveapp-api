using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AdminDtos;
using DatingLoveApp.Business.Dtos.AppUsers;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingLoveApp.Business.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<AppUser> _userManager;

    public AdminService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<PagedList<AppUserWithRolesDto>> GetUsersWithRolesAsync(
        UsersWithRolesParams usersWithRolesParams)
    {
        IQueryable<AppUser> query = _userManager.Users.AsQueryable();

        switch (usersWithRolesParams.SortBy)
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

        List<AppUserWithRolesDto> appUserWithRolesDtos = await query
            .AsNoTracking()
            .ProjectToType<AppUserWithRolesDto>() // no need to Include() and ThenInlude()
            .ToListAsync();

        PagedList<AppUserWithRolesDto> pagedList = new PagedList<AppUserWithRolesDto>
        {
            PageNumber = usersWithRolesParams.PageNumber,
            PageSize = usersWithRolesParams.PageSize,
            TotalRecords = totalRecords,
            Items = appUserWithRolesDtos
        };

        return pagedList;
    }
}
