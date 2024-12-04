using Application.ArchivedPosts.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.ArchivedPosts;
using Domain.Posts;
using MediatR;

namespace Application.ArchivedPosts.Commands;

public record CreateArchivedPostCommand : IRequest<Result<ArchivedPost, ArchivedPostException>>
{
    public Guid PostId { get; init; }
}

public class CreateArchivedPostCommandHandler(
    IArchivedPostRepository archivedPostRepository,
    IPostRepository postRepository)
    : IRequestHandler<CreateArchivedPostCommand, Result<ArchivedPost, ArchivedPostException>>
{
    public async Task<Result<ArchivedPost, ArchivedPostException>> Handle(
        CreateArchivedPostCommand request,
        CancellationToken cancellationToken)
    {
        var postId = new PostId(request.PostId);

        var post = await postRepository.GetById(postId, cancellationToken);

        return await post.Match(
            async p => await CreateEntity(p.Id, cancellationToken),
            () => Task.FromResult<Result<ArchivedPost, ArchivedPostException>>(
                new ArchivedPostForPostNotFoundException(postId))
        );
    }

    private async Task<Result<ArchivedPost, ArchivedPostException>> CreateEntity(
        PostId postId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = ArchivedPost.New(ArchivedPostId.New(), postId);

            return await archivedPostRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ArchivedPostUnknownException(ArchivedPostId.Empty(), exception);
        }
    }
}