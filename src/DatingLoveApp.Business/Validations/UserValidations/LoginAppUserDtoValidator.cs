using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.UserValidations;

public class LoginAppUserDtoValidator : AbstractValidator<LoginAppUserDto>
{
    public LoginAppUserDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
