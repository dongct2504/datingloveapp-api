using DatingLoveApp.Business.Common.Constants;
using DatingLoveApp.Business.Common.Errors;
using DatingLoveApp.Business.Dtos;
using DatingLoveApp.Business.Dtos.LocalUserDtos;
using DatingLoveApp.Business.Interfaces;
using DatingLoveApp.DataAccess.Entities;
using DatingLoveApp.DataAccess.Extensions;
using DatingLoveApp.DataAccess.Interfaces;
using FluentResults;
using MapsterMapper;
using Serilog;

namespace DatingLoveApp.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<LocalUserDto>> GetAllAsync(int page)
    {
        QueryOptions<LocalUser> options = new QueryOptions<LocalUser>
        {
            PageNumber = page,
            PageSize = PageSizeConstants.Default50
        };

        PagedList<LocalUserDto> pagedList = new PagedList<LocalUserDto>
        {
            PageNumber = page,
            PageSize = PageSizeConstants.Default50,
            Items = _mapper.Map<IEnumerable<LocalUserDto>>(await _userRepository.GetAllAsync(options)),
            TotalRecords = await _userRepository.GetCountAsync()
        };

        return pagedList;
    }

    public async Task<Result<LocalUserDto>> GetByIdAsync(Guid id)
    {
        QueryOptions<LocalUser> options = new QueryOptions<LocalUser>
        {
            Where = lc => lc.LocalUserId == id
        };

        LocalUser? user = await _userRepository.GetAsync(options);
        if (user == null)
        {
            string message = "User not found.";
            Log.Warning($"{nameof(GetByIdAsync)} - {message} - {typeof(UserService)}");
            return Result.Fail(new NotFoundError(message));
        }

        return _mapper.Map<LocalUserDto>(user);
    }

    public Task<Result<LocalUserDto>> AddAsync(LocalUserDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Guid id, CreateLocalUserDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveAsync(UpdateLocalUserDto userDto)
    {
        throw new NotImplementedException();
    }
}
