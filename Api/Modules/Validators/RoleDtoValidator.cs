using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class RoleDtoValidator: AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}