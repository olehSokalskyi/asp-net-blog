using FluentValidation;

namespace Application.Users.Commands;

public class LoginUserCommandValidator: AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.email).NotEmpty().EmailAddress();
        //RuleFor(x => x.password).NotEmpty().MinimumLength(8);
    }
}