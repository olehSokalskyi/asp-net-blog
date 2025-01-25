using FluentValidation;

namespace Application.Roles.Commands;

public class DeleteRoleCommandValidator: AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty();
    }
}