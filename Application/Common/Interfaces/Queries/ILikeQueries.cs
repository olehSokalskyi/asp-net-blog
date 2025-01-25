using Domain.Likes;
using Domain.Posts;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ILikeQueries
{
    Task<IReadOnlyList<Like>> GetAll(CancellationToken cancellationToken);
    Task<Option<Like>> GetById(LikeId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Like>> GetByUserId(UserId userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Like>> GetByPostId(PostId postId, CancellationToken cancellationToken);}