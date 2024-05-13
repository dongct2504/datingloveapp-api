using DatingLoveApp.Business.Dtos.LocalUserDtos;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.UserValidations;

public class UpdateLocalUserDtoValidator : AbstractValidator<UpdateLocalUserDto>
{
    public UpdateLocalUserDtoValidator()
    {
        RuleFor(x => x.LocalUserId)
            .Empty();

        RuleFor(x => x.UserName)
            .Empty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}
