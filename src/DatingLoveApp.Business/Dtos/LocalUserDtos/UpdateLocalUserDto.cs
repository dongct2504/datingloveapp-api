namespace DatingLoveApp.Business.Dtos.LocalUserDtos;

public class UpdateLocalUserDto
{
    public Guid LocalUserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string? Introduction { get; set; }

    public string? Interest { get; set; }

    public string? IdealType { get; set; }

    public string? Address { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? Role { get; set; }
}
