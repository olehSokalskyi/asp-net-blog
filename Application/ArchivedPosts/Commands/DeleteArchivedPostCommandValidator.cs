using FluentValidation;

namespace Application.ArchivedPosts.Commands;

public class DeleteArchivedPostCommandValidator : AbstractValidator<DeleteArchivedPostCommand>
{
    public DeleteArchivedPostCommandValidator()
    {
        RuleFor(x => x.ArchivedPostsId).NotEmpty(); 
    }
}