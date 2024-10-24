using FluentResults;
using SocialChitChat.Business.Dtos;
using SocialChitChat.Business.Dtos.AdminDtos;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.Business.Dtos.PictureDtos;

namespace SocialChitChat.Business.Interfaces;

public interface IAdminService
{
    Task<PagedList<AppUserWithRolesDto>> GetUsersWithRolesAsync(UsersWithRolesParams usersWithRolesParams);

    Task<Result<string[]>> EditRolesAsync(string id, string roles);

    Task<PictureDto> PictureToModerateAsync();
}
