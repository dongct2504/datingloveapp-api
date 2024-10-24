using SocialChitChat.Business.Common.Constants;
using SocialChitChat.Business.Common.Enums;
using SocialChitChat.DataAccess.Common;
using System.Text.RegularExpressions;

namespace SocialChitChat.Business.Validations;

public static class ValidateForRequest
{
    public static bool BeValidPhoneNumber(string phoneNumber)
    {
        phoneNumber = phoneNumber.Trim();

        Regex regex = new Regex(@"^\d{6,15}$");
        return regex.IsMatch(phoneNumber);
    }

    public static bool BeValidRole(string? role)
    {
        if (role == null)
        {
            return true;
        }

        return role == RoleConstants.User ||
            role == RoleConstants.Employee ||
            role == RoleConstants.Admin;
    }

    public static bool BeValidAge(DateTime date)
    {
        if (date == default)
        {
            return false;
        }

        int minAge = 16, maxAge = 120;
        int age = DateTime.Today.Year - date.Year;
        if (date > DateTime.Today.AddYears(-age)) // 2003/8/20 > 2003/5/19
        {
            age--;
        }

        return age >= minAge && age <= maxAge;
    }

    public static bool BeValidGender(GenderEnums gender)
    {
        return gender == GenderEnums.Male ||
            gender == GenderEnums.Female ||
            gender == GenderEnums.Unknown;
    }
}
