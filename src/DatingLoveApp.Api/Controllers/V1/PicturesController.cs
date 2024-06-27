using Asp.Versioning;
using DatingLoveApp.Business.Dtos.PictureDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingLoveApp.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/pictures")]
public class PicturesController : ApiController
{
    private readonly IPictureService _pictureService;

    public PicturesController(IPictureService pictureService)
    {
        _pictureService = pictureService;
    }

    [HttpPost("upload-picture")]
    [ProducesResponseType(typeof(PictureDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadPicture(IFormFile imageFile)
    {
        string id = User.GetCurrentUserId();

        Result<PictureDto> result = await _pictureService.UploadPictureAsync(id, imageFile);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPut("set-main-picture/{pictureId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetMainPicture(Guid pictureId)
    {
        string id = User.GetCurrentUserId();

        Result result = await _pictureService.SetMainPictureAsync(id, pictureId);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return NoContent();
    }

    [HttpDelete("remove-picture/{pictureId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePicture(Guid pictureId)
    {
        string id = User.GetCurrentUserId();

        Result result = await _pictureService.RemovePictureAsync(id, pictureId);
        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return NoContent();
    }
}
