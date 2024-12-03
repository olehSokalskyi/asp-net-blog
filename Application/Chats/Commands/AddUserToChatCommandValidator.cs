using FluentValidation;

namespace Application.Chats.Commands;

public class AddUserToChatCommandValidator: AbstractValidator<AddUserToChatCommand>
{
    public AddUserToChatCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ChatId).NotEmpty();
        RuleFor(x => x.InvitorId).NotEmpty();
    }
}