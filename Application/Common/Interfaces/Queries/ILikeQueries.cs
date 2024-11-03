using Domain.Likes;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ILikeQueries
{
    Task<IReadOnlyList<Like>> GetAll(CancellationToken cancellationToken);
    Task<Option<Like>> GetById(LikeId id, CancellationToken cancellationToken);
}