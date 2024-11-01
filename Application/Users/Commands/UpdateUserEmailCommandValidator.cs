using FluentValidation;

namespace Application.Users.Commands;

public class UpdateUserEmailCommandValidator: AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailCommandValidator()
    {
        RuleFor(x=> x.UserId).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}