using FluentValidation;
using SocialChitChat.Business.Dtos.AppUsers;

namespace SocialChitChat.Business.Validations.UserValidations;

public class UpdateAppUserDtoValidator : AbstractValidator<UpdateAppUserDto>
{
    public UpdateAppUserDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
