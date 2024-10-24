using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SocialChitChat.Business.Common;
using SocialChitChat.Business.Common.Mapping;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.Business.Services;

namespace SocialChitChat.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddMappings();

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPictureService, PictureService>();
        services.AddScoped<IAppUserLikeService, AppUserLikeService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IAdminService, AdminService>();

        services.AddScoped<LogUserActivity>();

        return services;
    }
}
