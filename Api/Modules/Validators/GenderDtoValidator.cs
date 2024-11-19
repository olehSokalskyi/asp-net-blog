using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class GenderDtoValidator : AbstractValidator<GenderDto>
{
    public GenderDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}