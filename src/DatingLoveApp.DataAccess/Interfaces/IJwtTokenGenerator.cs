using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.Business.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(LocalUser user);
    string GenerateEmailConfirmationToken(LocalUser user);
}
