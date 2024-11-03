using FluentValidation;

namespace Application.Likes.Commands;

public class DeleteLikeCommandValidator : AbstractValidator<DeleteLikeCommand>
{
    public DeleteLikeCommandValidator()
    {
        RuleFor(x => x.LikeId).NotEmpty();  
    }
}