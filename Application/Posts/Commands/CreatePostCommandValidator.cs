using FluentValidation;

namespace Application.Posts.Commands;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Body).NotEmpty()
            .MaximumLength(1500)
            .MinimumLength(3);
    }
}