using DatingLoveApp.Business.Common;
using DatingLoveApp.Business.Common.Mapping;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.Business.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DatingLoveApp.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddMappings();

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPictureService, PictureService>();

        return services;
    }
}
