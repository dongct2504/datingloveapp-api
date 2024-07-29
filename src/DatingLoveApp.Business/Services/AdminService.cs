using DatingLoveApp.Business.Dtos.AppUsers;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Identity;
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

    public async Task<List<AppUserDto>> GetUsersWithRolesAsync()
    {
        throw new NotImplementedException();
    }
}
