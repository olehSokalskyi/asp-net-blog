using FluentValidation;

namespace Application.Subscribers.Commands;

public class DeleteSubscriberCommandValidator : AbstractValidator<DeleteSubscriberCommand>
{
    public DeleteSubscriberCommandValidator()
    {
        RuleFor(x => x.SubscriberId).NotEmpty();  
    }
}