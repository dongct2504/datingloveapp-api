using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Extensions;

namespace SocialChitChat.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/conversations")]
public class ConversationsController : ApiController
{
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetConversationsForUser()
    {
        return Ok(await _conversationService.GetConversationsForUserAsync(User.GetCurrentUserId()));
    }
}
