using Domain.Likes;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ILikeRepository
{
    Task<Option<Like>> GetById(LikeId id, CancellationToken cancellationToken);
    
    Task<Like> Add(Like like, CancellationToken cancellationToken);
    Task<Like> Delete(Like like, CancellationToken cancellationToken);
}