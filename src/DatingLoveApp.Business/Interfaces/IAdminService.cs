using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AdminDtos;
using DatingLoveApp.Business.Dtos.AppUsers;
using DatingLoveApp.Business.Dtos.PictureDtos;
using FluentResults;

namespace DatingLoveApp.Business.Interfaces;

public interface IAdminService
{
    Task<PagedList<AppUserWithRolesDto>> GetUsersWithRolesAsync(UsersWithRolesParams usersWithRolesParams);

    Task<Result<string[]>> EditRolesAsync(string id, string roles);

    Task<PictureDto> PictureToModerateAsync();
}
