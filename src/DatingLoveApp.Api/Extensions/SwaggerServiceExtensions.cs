using Microsoft.OpenApi.Models;

namespace DatingLoveApp.Api.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerDocument(this IServiceCollection services)
    {
        services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DatingLoveApp API V1",
                Version = "v1"
            });

            swaggerGenOptions.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "DatingLoveApp API V2",
                Version = "v2"
            });

            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT authorization header using Bearer Scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocument(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "DatingLoveApp API V1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "DatingLoveApp API V2");
        });

        return app;
    }
}
