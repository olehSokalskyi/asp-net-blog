using FluentValidation;

namespace Application.Genders.Commands;

public class UpdateGenderCommandValidator : AbstractValidator<UpdateGenderCommand>
{
    public UpdateGenderCommandValidator()
    {
        RuleFor(x => x.GenderId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}