using Asp.Versioning;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.AdminDtos;
using DatingLoveApp.Business.Dtos.AppUsers;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/admin")]
public class AdminController : ApiController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users-with-roles")]
    [Authorize(Policy = PolicyConstants.RequiredAdminRole)]
    [ProducesResponseType(typeof(PagedList<AppUserWithRolesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersWithRoles([FromQuery] UsersWithRolesParams usersWithRolesParams)
    {
        return Ok(await _adminService.GetUsersWithRolesAsync(usersWithRolesParams));
    }

    [HttpGet("picture-to-moderate")]
    [Authorize(Policy = PolicyConstants.ModeratePictureRole)]
    public async Task<IActionResult> PictureToModerate()
    {
        return Ok("admin or employee can see this");
    }
}
