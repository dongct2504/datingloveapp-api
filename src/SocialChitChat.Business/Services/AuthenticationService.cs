using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Errors;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.Business.Dtos.AuthenticationDtos;
using SocialChitChat.Business.Interfaces;
using SocialChitChat.DataAccess.Common;
using SocialChitChat.DataAccess.Identity;
using SocialChitChat.DataAccess.Interfaces;

namespace SocialChitChat.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IMapper mapper,
        IJwtTokenGenerator jwtTokenGenerator,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IDateTimeProvider dateTimeProvider,
        ILogger<AuthenticationService> logger)
    {
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<AuthenticationDto>> RegisterAsync(RegisterAppUserDto userDto)
    {
        AppUser? user = await _userManager.Users
            .Where(u => u.UserName == userDto.UserName || u.Email == userDto.Email)
            .FirstOrDefaultAsync();
        if (user != null)
        {
            _logger.LogWarning($"{nameof(RegisterAsync)} - {ErrorMessageConsts.UserDuplicate} - {typeof(AuthenticationService)}");
            return Result.Fail(new ConflictError(ErrorMessageConsts.UserDuplicate));
        }

        user = _mapper.Map<AppUser>(userDto);
        user.LastActive = _dateTimeProvider.LocalVietnamDateTimeNow;
        user.CreatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;
        user.UpdatedAt = _dateTimeProvider.LocalVietnamDateTimeNow;

        IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
        {
            var errorMessages = result.Errors.Select(e => e.Description);

            string message = string.Join(" ", errorMessages);
            _logger.LogWarning($"{nameof(RegisterAsync)} - {ErrorMessageConsts.WrongUserName} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.WrongUserName));
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
        AppUser? user = await _userManager.Users
            .Include(u => u.Pictures)
            .Where(u => u.UserName == userDto.UserName)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            _logger.LogWarning($"{nameof(LoginAsync)} - {ErrorMessageConsts.WrongUserName} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.WrongUserName));
        }

        // check if the email is confirm here

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
        if (!signInResult.Succeeded)
        {
            _logger.LogWarning($"{nameof(LoginAsync)} - {ErrorMessageConsts.WrongPassword} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(ErrorMessageConsts.WrongPassword));
        }

        AppUserDto appUserDto = _mapper.Map<AppUserDto>(user);
        string token = await _jwtTokenGenerator.GenerateTokenAsync(user);

        AuthenticationDto authenticationDto = new AuthenticationDto
        {
            AppUserDto = appUserDto,
            Token = token
        };

        _logger.LogInformation($"User login at: {_dateTimeProvider.LocalVietnamDateTimeNow:dd/MM/yyyy hh:mm tt}.");

        return authenticationDto;
    }
}
