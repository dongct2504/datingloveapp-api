using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Extensions;
using DatingLoveApp.DataAccess.Interfaces;
using FluentResults;
using MapsterMapper;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICacheService _cacheService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IUserRepository userRepository,
        IMapper mapper,
        IJwtTokenGenerator jwtTokenGenerator,
        ICacheService cacheService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _cacheService = cacheService;
    }

    public async Task<Result<AuthenticationDto>> RegisterAsync(RegisterLocalUserDto userDto)
    {
        LocalUser? user = await _userRepository.GetAsync(new QueryOptions<LocalUser>
        {
            Where = lc => lc.UserName == userDto.UserName || lc.Email == userDto.Email
        });
        if (user != null)
        {
            string message = "User is already exist.";
            Log.Warning($"{nameof(RegisterAsync)} - {message} - {typeof(AuthenticationService)}");
            return Result.Fail(new ConflictError(message));
        }

        user = _mapper.Map<LocalUser>(userDto);
        user.LocalUserId = Guid.NewGuid();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        user.Role ??= RoleConstants.Customer;
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;

        await _userRepository.AddAsync(user);

        //string token = _jwtTokenGenerator.GenerateEmailConfirmationToken(user);

        return _mapper.Map<AuthenticationDto>((user, "token"));
    }

    public async Task<Result<AuthenticationDto>> LoginAsync(LoginLocalUserDto userDto)
    {
        LocalUser? user = await _userRepository.GetAsync(new QueryOptions<LocalUser>
        {
            Where = u => u.UserName == userDto.UserName
        });
        if (user == null)
        {
            string message = "The user name is incorrect.";
            Log.Warning($"{nameof(LoginAsync)} - {message} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(message));
        }

        bool verified = BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash);
        if (!verified)
        {
            string message = "The password is incorrect.";
            Log.Warning($"{nameof(LoginAsync)} - {message} - {typeof(AuthenticationService)}");
            return Result.Fail(new BadRequestError(message));
        }

        // check if the email is confirm here

        string token = _jwtTokenGenerator.GenerateToken(user);

        Log.Information($"User login at: {DateTime.Now:dd/MM/yyyy hh:mm tt}.");

        return _mapper.Map<AuthenticationDto>((user, token));
    }
}
