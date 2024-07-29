using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AdminDtos;
using DatingLoveApp.Business.Dtos.AppUsers;

namespace DatingLoveApp.Business.Interfaces;

public interface IAdminService
{
    Task<PagedList<AppUserWithRolesDto>> GetUsersWithRolesAsync(UsersWithRolesParams usersWithRolesParams);
}
