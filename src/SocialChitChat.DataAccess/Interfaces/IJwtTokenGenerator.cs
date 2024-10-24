using SocialChitChat.DataAccess.Identity;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(AppUser user);
    string GenerateEmailConfirmationToken(AppUser user);
}
