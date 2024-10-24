namespace SocialChitChat.Business.Dtos.AppUsers;

public class UpdateAppUserDto
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Introduction { get; set; }

    public string? Interest { get; set; }

    public string? IdealType { get; set; }

    public string? Address { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }
}
