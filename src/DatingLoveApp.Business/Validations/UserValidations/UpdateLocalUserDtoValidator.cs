using DatingLoveApp.Business.Dtos.LocalUserDtos;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.UserValidations;

public class UpdateLocalUserDtoValidator : AbstractValidator<UpdateLocalUserDto>
{
    public UpdateLocalUserDtoValidator()
    {
        RuleFor(x => x.LocalUserId)
            .NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Must(ValidateForRequest.BeValidPhoneNumber).WithMessage("Phone number is invalid.");

        RuleFor(x => x.Role)
            .Must(ValidateForRequest.BeValidRole).WithMessage("Role is invalid.");
    }
}
