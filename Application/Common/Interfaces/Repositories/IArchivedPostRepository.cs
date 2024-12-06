using Domain.ArchivedPosts;
using Domain.Posts;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IArchivedPostRepository
{
    Task<Option<ArchivedPost>> GetById(ArchivedPostId id, CancellationToken cancellationToken);
    Task<Option<ArchivedPost>> GetByPostId(PostId postId, CancellationToken cancellationToken);

    Task<Option<ArchivedPost>> GetByArchivedPostAndUserId(ArchivedPostId id, UserId userId,
        CancellationToken cancellationToken);

    Task<ArchivedPost> Add(ArchivedPost archivedPost, CancellationToken cancellationToken);
    Task<ArchivedPost> Delete(ArchivedPost archivedPost, CancellationToken cancellationToken);
}