using FluentValidation;

namespace Application.Comments.Commands;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();

        RuleFor(x => x.Body).NotEmpty()
            .MaximumLength(1500)
            .MinimumLength(3);

        RuleFor(x => x.UserId).NotEmpty();
    }
}