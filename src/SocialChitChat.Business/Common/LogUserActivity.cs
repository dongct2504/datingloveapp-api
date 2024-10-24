using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using SocialChitChat.DataAccess.Extensions;
using SocialChitChat.DataAccess.Interfaces;
using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.Business.Common;

public class LogUserActivity : IAsyncActionFilter
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LogUserActivity(UserManager<AppUser> userManager, IDateTimeProvider dateTimeProvider)
    {
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ActionExecutedContext resultContext = await next();

        if (resultContext.HttpContext.User.Identity == null ||
            !resultContext.HttpContext.User.Identity.IsAuthenticated)
        {
            return;
        }

        string userId = resultContext.HttpContext.User.GetCurrentUserId();

        AppUser? user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            user.LastActive = _dateTimeProvider.LocalVietnamDateTimeNow;
            await _userManager.UpdateAsync(user);
        }
    }
}
