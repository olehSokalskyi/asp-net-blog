using FluentValidation;

namespace Application.ArchivedPosts.Commands;

public class CreateArchivedPostCommandValidator : AbstractValidator<CreateArchivedPostCommand>
{
    public CreateArchivedPostCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
    }
}