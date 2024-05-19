using DatingLoveApp.DataAccess.Entities;

namespace DatingLoveApp.DataAccess.Extensions;

public static class LocalUserExtensions
{
    public static int GetAge(this DateTime dateOfBirth)
    {
        int age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth > DateTime.Today.AddYears(-age)) // 2003/8/20 > 2003/5/19
        {
            age--;
        }

        return age;
    }
}
