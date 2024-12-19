using FluentValidation;
using SocialChitChat.Business.Dtos.GroupChatDtos;

namespace SocialChitChat.Business.Validations.GroupChatValidations;

public class CreateGroupChatDtoValidator : AbstractValidator<CreateGroupChatDto>
{
    public CreateGroupChatDtoValidator()
    {
        RuleFor(x => x.AdminId)
            .NotEmpty();

        RuleFor(x => x.GroupName)
            .NotEmpty();

        RuleFor(x => x.ParticipantIds)
            .NotEmpty();
    }
}
