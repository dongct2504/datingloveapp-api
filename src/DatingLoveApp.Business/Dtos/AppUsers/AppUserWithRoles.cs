namespace DatingLoveApp.Business.Dtos.AppUsers;

public class AppUserWithRoles
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public List<string> Roles { get; set; } = new List<string>();
}
