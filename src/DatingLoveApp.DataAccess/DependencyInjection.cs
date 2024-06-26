using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Common;
using DatingLoveApp.DataAccess.Identity;
using DatingLoveApp.DataAccess.Interfaces;
using DatingLoveApp.DataAccess.Repositories;
using DatingLoveApp.DataAccess.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingLoveApp.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // register redis
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = configuration.GetConnectionString("Cache"));

        services.AddSingleton<ICacheService, CacheService>();

        // register image service
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SectionName));
        services.AddScoped<IFileStorageService, FileStorageService>();

        services.AddIdentity();
        services.AddAuth(configuration);

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPictureRepository, PictureRepository>();

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 3;

            // User settings
            options.User.RequireUniqueEmail = true;

            // Sign-in settings
            //options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<DatingLoveAppIdentityDbContext>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddSignInManager<SignInManager<AppUser>>()
        .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        JwtSettings jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        });

        return services;
    }
}
