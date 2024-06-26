using Asp.Versioning;
using DatingLoveApp.Business.Dtos.PictureDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/pictures")]
public class PicturesController : ApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(PictureDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Upload(IFormFile picture)
    {
        return Ok();
    }
}
