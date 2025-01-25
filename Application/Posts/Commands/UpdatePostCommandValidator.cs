using FluentValidation;

namespace Application.Posts.Commands;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
        
        RuleFor(x => x.Body).NotEmpty()
            .MaximumLength(1500)
            .MinimumLength(3);
        
        RuleFor(x => x.UserId).NotEmpty();
    }
}