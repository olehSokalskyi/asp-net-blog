using Application.Comments.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Comments;
using Domain.Users;
using MediatR;

namespace Application.Comments.Commands;

public record UpdateCommentCommand : IRequest<Result<Comment, CommentException>>
{
    public required Guid CommentId { get; init; }
    public required string Body { get; init; }
    public required Guid UserId { get; init; }
}

public class UpdateCommentCommandHandler(
    ICommentRepository commentRepository) : IRequestHandler<UpdateCommentCommand, Result<Comment, CommentException>>
{
    public async Task<Result<Comment, CommentException>> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var commentId = new CommentId(request.CommentId);
        var userId = new UserId(request.UserId);

        var existingPost = await commentRepository.GetByCommentAndUserId(commentId, userId, cancellationToken);

        return await existingPost.Match(
            async p => await UpdateEntity(p, request.Body, cancellationToken),
            () => Task.FromResult<Result<Comment, CommentException>>(new CommentNotFoundException(commentId)));
    }

    private async Task<Result<Comment, CommentException>> UpdateEntity(
        Comment entity,
        string body,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(body);

            return await commentRepository.Update(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new CommentUnknownException(CommentId.Empty(), e);
        }
    }
}