using Asp.Versioning;
using DatingLoveApp.Api.Middleware;
using DatingLoveApp.Api.Services;
using DatingLoveApp.Business;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess;
using DatingLoveApp.DataAccess.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

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

    // versioning the api
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    // register dependencies in other players
    builder.Services.AddSingleton<IFileStorageService, FileStorageService>();
    builder.Services
        .AddBusiness()
        .AddDataAccess();
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

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
