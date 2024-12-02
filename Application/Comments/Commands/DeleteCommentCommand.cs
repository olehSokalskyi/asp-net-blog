using Application.Comments.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Comments;
using Domain.Users;
using MediatR;

namespace Application.Comments.Commands;

public record DeleteCommentCommand : IRequest<Result<Comment, CommentException>>
{
    public required Guid CommentId { get; init; }
    public required Guid UserId { get; init; }
}

public class DeleteCommentCommandHandler(
    ICommentRepository commentRepository) : IRequestHandler<DeleteCommentCommand, Result<Comment, CommentException>>
{
    public async Task<Result<Comment, CommentException>> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken)
    {
        var commentId = new CommentId(request.CommentId);
        var userId = new UserId(request.UserId);

        var existingComment = await commentRepository.GetByCommentAndUserId(commentId, userId, cancellationToken);

        return await existingComment.Match(
            async p => await DeleteEntity(p, cancellationToken),
            () => Task.FromResult<Result<Comment, CommentException>>(new CommentNotFoundException(commentId)));
    }

    private async Task<Result<Comment, CommentException>> DeleteEntity(
        Comment comment,
        CancellationToken cancellationToken)
    {
        try
        {
            return await commentRepository.Delete(comment, cancellationToken);
        }
        catch (Exception e)
        {
            return new CommentUnknownException(CommentId.Empty(), e);
        }
    }
}