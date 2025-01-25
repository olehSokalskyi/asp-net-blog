using FluentValidation;

namespace Application.Categories.Commands;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);
    }
}