using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Posts.Exceptions;
using Domain.Posts;
using Domain.Users;
using MediatR;

namespace Application.Posts.Commands;

public record DeletePostCommand : IRequest<Result<Post, PostException>>
{
    public required Guid PostId { get; init; }
    public required Guid UserId { get; init; }
}

public class DeletePostCommandHandler(
    IPostRepository postRepository) : IRequestHandler<DeletePostCommand, Result<Post, PostException>>
{
    public async Task<Result<Post, PostException>> Handle(
        DeletePostCommand request,
        CancellationToken cancellationToken)
    {
        var postId = new PostId(request.PostId);
        var userId = new UserId(request.UserId);

        var existingPost = await postRepository.GetByPostAndUserId(postId, userId, cancellationToken);

        return await existingPost.Match(
            async p => await DeleteEntity(p, cancellationToken),
            () => Task.FromResult<Result<Post, PostException>>(new PostNotFoundException(postId)));
    }

    private async Task<Result<Post, PostException>> DeleteEntity(
        Post post,
        CancellationToken cancellationToken)
    {
        try
        {
            return await postRepository.Delete(post, cancellationToken);
        }
        catch (Exception e)
        {
            return new PostUnknownException(PostId.Empty(), e);
        }
    }
}