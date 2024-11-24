using Domain.Likes;
using Optional;

namespace Application.Common.Interfaces;

public interface ILikeRepository
{
    Task<Like> Add(Like like, CancellationToken cancellationToken);
    Task<Like> Delete(Like like, CancellationToken cancellationToken);
    Task<Option<Like>> GetById(LikeId id, CancellationToken cancellationToken);
}