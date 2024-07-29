using DatingLoveApp.Business.Dtos.AppUsers;

namespace DatingLoveApp.Business.Interfaces;

public interface IAdminService
{
    Task<List<AppUserDto>> GetUsersWithRolesAsync();
}
