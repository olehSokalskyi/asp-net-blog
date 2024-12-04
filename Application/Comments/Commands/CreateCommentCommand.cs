using Application.Comments.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Comments;
using Domain.Posts;
using Domain.Users;
using MediatR;

namespace Application.Comments.Commands;

public record CreateCommentCommand : IRequest<Result<Comment, CommentException>>
{
    public required string Body { get; init; }
    public required Guid UserId { get; init; }
    public required Guid PostId { get; init; }
}

public class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IUserRepository userRepository,
    IPostRepository postRepository) : IRequestHandler<CreateCommentCommand, Result<Comment, CommentException>>
{
    public async Task<Result<Comment, CommentException>> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var postId = new PostId(request.PostId);

        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match(
            async _ =>
            {
                var existingPost = await postRepository.GetById(postId, cancellationToken);

                return await existingPost.Match(
                    async _ => await CreateEntity(request.Body, userId, postId, cancellationToken),
                    () => Task.FromResult<Result<Comment, CommentException>>(
                        new CommentPostNotFoundException(CommentId.Empty(), postId)));
            },
            () => Task.FromResult<Result<Comment, CommentException>>(
                new CommentUserNotFoundException(CommentId.Empty(), userId)));
    }

    private async Task<Result<Comment, CommentException>> CreateEntity(
        string body,
        UserId userId,
        PostId postId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Comment.New(CommentId.New(), body, userId, postId);

            return await commentRepository.Add(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new CommentUnknownException(CommentId.Empty(), e);
        }
    }
}