using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.UserValidations;

public class RegisterLocalUserDtoValidator : AbstractValidator<RegisterLocalUserDto>
{
    public RegisterLocalUserDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
