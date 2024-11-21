using Application.ArchivedPosts.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.ArchivedPosts;
using MediatR;

namespace Application.ArchivedPosts.Commands;

public record DeleteArchivedPostCommand : IRequest<Result<ArchivedPost, ArchivedPostException>>
{
    public required Guid ArchivedPostsId { get; init; }
}

public class DeleteArchivedPostCommandHandler(IArchivedPostRepository archivedPostRepository)
    : IRequestHandler<DeleteArchivedPostCommand, Result<ArchivedPost, ArchivedPostException>>
{
    public async Task<Result<ArchivedPost, ArchivedPostException>> Handle(
        DeleteArchivedPostCommand request,
        CancellationToken cancellationToken)
    {
        var archivedPostId = new ArchivedPostId(request.ArchivedPostsId);

        var existingArchivedPost = await archivedPostRepository.GetById(archivedPostId, cancellationToken);

        return await existingArchivedPost.Match<Task<Result<ArchivedPost, ArchivedPostException>>>(
            async a => await DeleteEntity(a, cancellationToken),
            () => Task.FromResult<Result<ArchivedPost, ArchivedPostException>>(new ArchivedPostNotFoundException(archivedPostId)));
    }

    public async Task<Result<ArchivedPost, ArchivedPostException>> DeleteEntity(ArchivedPost archivedPost, CancellationToken cancellationToken)
    {
        try
        {
            return await archivedPostRepository.Delete(archivedPost, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ArchivedPostUnknownException(archivedPost.Id, exception);
        }
    }
}