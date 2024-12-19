using FluentValidation;
using SocialChitChat.Business.Dtos.PostDtos;

namespace SocialChitChat.Business.Validations.PostValidations;

public class UpdatePostValidation : AbstractValidator<UpdatePostDto>
{
    public UpdatePostValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}
