using FluentValidation;

namespace Application.Roles;

public class DeleteRoleCommandValidator: AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty();
    }
}