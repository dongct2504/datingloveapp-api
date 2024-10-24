namespace SocialChitChat.Business.Dtos.AuthenticationDtos;

public class RegisterAppUserDto
{
    public string UserName { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }
}
