using DatingLoveApp.Business.Common.Constants;
using System.Text.RegularExpressions;

namespace DatingLoveApp.Business.Validations;

public static class ValidateForRequest
{
    public static bool BeValidPhoneNumber(string phoneNumber)
    {
        Regex regex = new Regex(@"^\d{6,15}$");
        return regex.IsMatch(phoneNumber);
    }

    public static bool BeValidRole(string? role)
    {
        if (role == null)
        {
            return true;
        }

        return role == RoleConstants.Customer ||
            role == RoleConstants.Employee ||
            role == RoleConstants.Admin;
    }
}
