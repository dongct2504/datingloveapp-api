using Asp.Versioning;
using DatingLoveApp.Api.Middleware;
using DatingLoveApp.Business;
using DatingLoveApp.DataAccess;
using DatingLoveApp.DataAccess.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddCors();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(swaggerGenOptions =>
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

    // register serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom
        .Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog();

    // setting connection string and register DbContext
    var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
    {
        ConnectionString = builder.Configuration.GetConnectionString("DatingLoveAppConnectionString"),
        UserID = builder.Configuration["UserID"],
        Password = builder.Configuration["Password"]
    };

    builder.Services.AddDbContext<DatingLoveAppDbContext>(options =>
        options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString));

    // register dependencies in other players
    builder.Services
        .AddBusiness()
        .AddDataAccess(builder.Configuration);

    // versioning the api
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
        options.AddApiVersionParametersWhenVersionNeutral = true;
    });
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "DatingLoveApp API V1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "DatingLoveApp API V2");
        });
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();

    //app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseCors(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200", "http://datinglove.vutiendat3601.io.vn"));

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
