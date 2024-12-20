using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialChitChat.DataAccess.Common;
using SocialChitChat.DataAccess.Common.Enums;
using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.DataAccess.Initializers;

public class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        await SeedRolesAsync(roleManager);
        await SeedAdminAsync(userManager);
        await SeedEmployeesAsync(userManager);
        await SeedUsersAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
    {
        string[] roles = { RoleConstants.Admin, RoleConstants.User, RoleConstants.Employee };
        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole(role));
            }
        }
    }

    private static async Task SeedAdminAsync(UserManager<AppUser> userManager)
    {
        var adminEmail = "admin@gmail.com";
        if ((await userManager.FindByEmailAsync(adminEmail)) == null)
        {
            var adminUser = new AppUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                Nickname = "Administrator",
                Gender = (byte)GenderEnum.Male,
                DateOfBirth = DateTime.UtcNow.AddHours(7).AddYears(-30),
                LastActive = DateTime.UtcNow.AddHours(7)
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            }
        }
    }

    private static async Task SeedEmployeesAsync(UserManager<AppUser> userManager)
    {
        for (int i = 1; i <= 2; i++)
        {
            var userEmail = $"employee{i}@gmail.com";
            if ((await userManager.FindByEmailAsync(userEmail)) == null)
            {
                var user = new AppUser
                {
                    UserName = $"employee{i}",
                    Email = userEmail,
                    EmailConfirmed = true,
                    Nickname = $"Employee {i}",
                    Gender = (byte)(i % 2 == 0 ? GenderEnum.Male : GenderEnum.Female),
                    DateOfBirth = DateTime.UtcNow.AddHours(7).AddYears(-25 - i),
                    LastActive = DateTime.UtcNow.AddHours(7)
                };

                var result = await userManager.CreateAsync(user, "Employee@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RoleConstants.Employee);
                }
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        for (int i = 1; i <= 7; i++)
        {
            var userEmail = $"user{i}@gmail.com";
            if ((await userManager.FindByEmailAsync(userEmail)) == null)
            {
                var user = new AppUser
                {
                    UserName = $"user{i}",
                    Email = userEmail,
                    EmailConfirmed = true,
                    Nickname = $"User {i}",
                    Gender = (byte)(i == 5 || i == 7
                        ? GenderEnum.Unknown
                        : (i % 2 == 0 ? GenderEnum.Male : GenderEnum.Female)),
                    DateOfBirth = DateTime.UtcNow.AddHours(7).AddYears(-20 - i),
                    LastActive = DateTime.UtcNow.AddHours(7)
                };

                var result = await userManager.CreateAsync(user, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RoleConstants.User);
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
