using FluentValidation;

namespace Application.Genders.Commands;

public class DeleteGenderCommandValidator : AbstractValidator<DeleteGenderCommand>
{
    public DeleteGenderCommandValidator()
    {
        RuleFor(x => x.GenderId).NotEmpty();
    }
}