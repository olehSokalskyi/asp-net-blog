using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Posts.Exceptions;
using Domain.Posts;
using Domain.Users;
using MediatR;

namespace Application.Posts.Commands;

public record UpdatePostCommand : IRequest<Result<Post, PostException>>
{
    public required Guid PostId { get; init; }
    public required string Body { get; init; }
    public required Guid UserId { get; init; }
}

public class UpdatePostCommandHandler(IPostRepository postRepository)
    : IRequestHandler<UpdatePostCommand, Result<Post, PostException>>
{
    public async Task<Result<Post, PostException>> Handle(
        UpdatePostCommand request,
        CancellationToken cancellationToken)
    {
        var postId = new PostId(request.PostId);
        var userId = new UserId(request.UserId);

        var existingPost = await postRepository.GetByPostAndUserId(postId, userId, cancellationToken);

        return await existingPost.Match(
            async p => await UpdateEntity(p, request.Body, cancellationToken),
            () => Task.FromResult<Result<Post, PostException>>(new PostNotFoundException(postId)));
    }

    private async Task<Result<Post, PostException>> UpdateEntity(
        Post entity,
        string body,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(body);

            return await postRepository.Update(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new PostUnknownException(PostId.Empty(), e);
        }
    }
}