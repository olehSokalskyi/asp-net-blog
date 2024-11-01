using FluentValidation;

namespace Application.Users.Commands;

public class UpdateUserDataCommandValidator: AbstractValidator<UpdateUserDataCommand>
{
    public UpdateUserDataCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Username).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}