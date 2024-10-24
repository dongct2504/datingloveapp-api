using FluentValidation;
using SocialChitChat.Business.Dtos.AppUsers;
using SocialChitChat.Business.Validations;

namespace SocialChitChat.Business.Validations.UserValidations;

public class UpdateAppUserDtoValidator : AbstractValidator<UpdateAppUserDto>
{
    public UpdateAppUserDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Role)
            .Must(ValidateForRequest.BeValidRole)
            .WithMessage("Role is invalid.");
    }
}
