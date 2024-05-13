using DatingLoveApp.Business.Dtos.AuthenticationDtos;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.UserValidations;

public class LoginLocalUserDtoValidator : AbstractValidator<LoginLocalUserDto>
{
    public LoginLocalUserDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
