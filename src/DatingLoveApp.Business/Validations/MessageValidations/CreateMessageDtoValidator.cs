using DatingLoveApp.Business.Dtos.MessageDtos;
using FluentValidation;

namespace DatingLoveApp.Business.Validations.MessageValidations;

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
