using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Identity;
using DatingLoveApp.DataAccess.Interfaces;
using DatingLoveApp.DataAccess.Specifications.PictureSpecifications;
using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IPictureRepository _pictureRepository;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IMapper mapper,
        IJwtTokenGenerator jwtTokenGenerator,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager,
        IPictureRepository pictureRepository)
    {
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _pictureRepository = pictureRepository;
    }

    public async Task<Result<AuthenticationDto>> RegisterAsync(RegisterAppUserDto userDto)
    {
        AppUser? user = await _userManager.Users
            .Where(u => u.UserName == userDto.UserName || u.Email == userDto.Email)
            .FirstOrDefaultAsync();
        if (user != null)
        {
            string message = "User is already exist.";
            Log.Warning($"{nameof(RegisterAsync)} - {message} - {typeof(AuthenticationService)}");
            return Result.Fail(new ConflictError(message));
        }

        user = _mapper.Map<AppUser>(userDto);
        user.LastActive = DateTime.Now;
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;

        IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
        {
            var errorMessages = result.Errors.Select(e => e.Description);

            string message = string.Join(" ", errorMessages);
            Log.Warning($"{nameof(RegisterAsync)} - {message} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(message));
        }

        string[] roleNames = { RoleConstants.User, RoleConstants.Employee, RoleConstants.Admin };
        foreach (string roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        if (userDto.UserName == "admin")
        {
            await _userManager.AddToRoleAsync(user, RoleConstants.Admin);
        }
        else
        {
            if (string.IsNullOrEmpty(userDto.Role))
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.User);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, userDto.Role);
            }
        }

        AppUserDto appUserDto = _mapper.Map<AppUserDto>(user);

        //string token = _jwtTokenGenerator.GenerateEmailConfirmationToken(user);
        string token = "token";

        AuthenticationDto authenticationDto = new AuthenticationDto
        {
            AppUserDto = appUserDto,
            Token = token
        };

        return authenticationDto;
    }

    public async Task<Result<AuthenticationDto>> LoginAsync(LoginAppUserDto userDto)
    {
        AppUser user = await _userManager.FindByNameAsync(userDto.UserName);
        if (user == null)
        {
            string message = "The user name is incorrect.";
            Log.Warning($"{nameof(LoginAsync)} - {message} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(message));
        }

        // check if the email is confirm here

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
        if (!signInResult.Succeeded)
        {
            string message = "The password is incorrect.";
            Log.Warning($"{nameof(LoginAsync)} - {message} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(message));
        }

        AppUserDto appUserDto = _mapper.Map<AppUserDto>(user);

        var mainSpec = new MainPictureByUserIdSpecification(appUserDto.Id);
        appUserDto.ProfilePictureUrl = (await _pictureRepository.GetWithSpecAsync(mainSpec, true))?.ImageUrl;

        string token = await _jwtTokenGenerator.GenerateTokenAsync(user);

        AuthenticationDto authenticationDto = new AuthenticationDto
        {
            AppUserDto = appUserDto,
            Token = token
        };

        Log.Information($"User login at: {DateTime.Now:dd/MM/yyyy hh:mm tt}.");

        return authenticationDto;
    }
}
