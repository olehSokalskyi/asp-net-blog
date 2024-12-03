using FluentValidation;

namespace Application.Chats.Commands;

public class CheckUserIntoChatCommandValidator : AbstractValidator<CheckUserIntoChatCommand>
{
    public CheckUserIntoChatCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ChatId).NotEmpty();
    }
}