using Domain.ArchivedPosts;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IArchivedPostRepository
{
    Task<ArchivedPost> Add(ArchivedPost archivedPost, CancellationToken cancellationToken);
    Task<ArchivedPost> Delete(ArchivedPost archivedPost, CancellationToken cancellationToken);
    Task<Option<ArchivedPost>> GetById(ArchivedPostId id, CancellationToken cancellationToken);
}