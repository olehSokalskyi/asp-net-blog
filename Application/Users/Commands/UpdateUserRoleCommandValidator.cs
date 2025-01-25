using FluentValidation;

namespace Application.Users.Commands;

public class UpdateUserRoleCommandValidator: AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}