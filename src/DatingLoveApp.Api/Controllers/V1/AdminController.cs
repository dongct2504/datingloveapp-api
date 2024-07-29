using Asp.Versioning;
using DatingLoveApp.DataAccess.Common;
using DatingLoveApp.DataAccess.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/admin")]
public class AdminController : ApiController
{
    [HttpGet("users-with-roles")]
    [Authorize(Policy = PolicyConstants.RequiredAdminRole)]
    public async Task<IActionResult> GetUsersWithRoles()
    {
        return Ok("only admin can see this");
    }

    [HttpGet("picture-to-moderate")]
    [Authorize(Policy = PolicyConstants.ModeratePictureRole)]
    public async Task<IActionResult> PictureToModerate()
    {
        return Ok("admin or employee can see this");
    }
}
