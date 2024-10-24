using FluentValidation;
using SocialChitChat.Business.Dtos.AuthenticationDtos;

namespace SocialChitChat.Business.Validations.UserValidations;

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
