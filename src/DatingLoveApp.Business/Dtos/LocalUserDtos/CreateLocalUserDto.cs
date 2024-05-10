namespace DatingLoveApp.Business.Dtos.LocalUserDtos;

public class CreateLocalUserDto
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }
}
