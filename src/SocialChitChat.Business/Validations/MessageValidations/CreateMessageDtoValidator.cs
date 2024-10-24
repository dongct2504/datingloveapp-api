using FluentValidation;
using SocialChitChat.Business.Dtos.MessageDtos;

namespace SocialChitChat.Business.Validations.MessageValidations;

public class CreateMessageDtoValidator : AbstractValidator<CreateMessageDto>
{
    public CreateMessageDtoValidator()
    {
        RuleFor(x => x.RecipientId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}
