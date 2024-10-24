using DatingLoveApp.Business.Dtos.AppUsers;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.UserValidations;

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
