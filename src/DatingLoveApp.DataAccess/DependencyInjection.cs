using DatingLoveApp.DataAccess.Interfaces;
using DatingLoveApp.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DatingLoveApp.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
