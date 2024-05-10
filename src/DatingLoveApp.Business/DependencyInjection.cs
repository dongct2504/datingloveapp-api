using DatingLoveApp.Business.Common.Mapping;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DatingLoveApp.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddMappings();

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
