using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.UserValidations;

public class RegisterLocalUserDtoValidator : AbstractValidator<RegisterLocalUserDto>
{
    public RegisterLocalUserDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Nickname)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(128)
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Must(ValidateForRequest.BeValidPhoneNumber)
            .WithMessage("Phone number is invalid.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(ValidateForRequest.BeValidAge)
            .WithMessage("You must be older than 16 to be able to register.");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .Must(ValidateForRequest.BeValidGender)
            .WithMessage("Gender must be either 'Male', 'Female', or 'Unknown'.");

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.Role)
            .Must(ValidateForRequest.BeValidRole)
            .WithMessage("Role is invalid.");
    }
}
