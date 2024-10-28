using FluentValidation;
using SocialChitChat.Business.Dtos.ConversationDtos;

namespace SocialChitChat.Business.Validations.ConversationValidations;

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
