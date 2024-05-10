namespace DatingLoveApp.Business.Dtos.LocalUserDtos;

public class LocalUserDto
{
    public Guid LocalUserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsConfirmEmail { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public bool IsConfirmPhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? LockoutEndDate { get; set; }

    public bool LockoutEnable { get; set; }

    public string? Role { get; set; }

    public string? ImageUrl { get; set; }

    public string? Address { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }
}
