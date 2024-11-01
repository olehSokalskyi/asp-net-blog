using FluentValidation;

namespace Application.Genders.Commands;

public class CreateGenderCommandValidator : AbstractValidator<CreateGenderCommand>
{
    public CreateGenderCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}