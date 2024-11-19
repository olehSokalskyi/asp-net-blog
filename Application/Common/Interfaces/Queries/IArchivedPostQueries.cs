using Domain.ArchivedPosts;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IArchivedPostQueries
{
    Task<IReadOnlyList<ArchivedPost>> GetAll(CancellationToken cancellationToken);
    Task<Option<ArchivedPost>> GetById(ArchivedPostId id, CancellationToken cancellationToken);
}