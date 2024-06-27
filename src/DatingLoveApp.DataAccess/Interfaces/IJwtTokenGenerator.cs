using DatingLoveApp.DataAccess.Identity;

namespace DatingLoveApp.Business.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(AppUser user);
    string GenerateEmailConfirmationToken(AppUser user);
}
