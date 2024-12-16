using Domain.Posts;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IPostQueries
{
    Task<IReadOnlyList<Post>> GetAll(CancellationToken cancellationToken);
    Task<Option<Post>> GetById(PostId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Post>> GetByUserId(UserId userId, CancellationToken cancellationToken);
}