using FluentValidation;
using SocialChitChat.Business.Dtos.PostDtos;

namespace SocialChitChat.Business.Validations.PostValidations;

public class CreatePostDtoValidation : AbstractValidator<CreatePostDto>
{
    public CreatePostDtoValidation()
    {
        RuleFor(x => x.Content)
            .NotEmpty();
    }
}
