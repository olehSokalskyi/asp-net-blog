using FluentValidation;

namespace Application.Subscribers.Commands;

public class CreateSubscriberCommandValidator : AbstractValidator<CreateSubscriberCommand>
{
    public CreateSubscriberCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FollowUserId).NotEmpty();
    }
}