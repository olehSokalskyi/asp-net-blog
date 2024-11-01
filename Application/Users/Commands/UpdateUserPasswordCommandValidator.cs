using FluentValidation;

namespace Application.Users.Commands;

public class UpdateUserPasswordCommandValidator: AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MaximumLength(16).MinimumLength(8);
        RuleFor(x=> x.NewPassword).NotEmpty().MaximumLength(16).MinimumLength(8);
        
    }
}