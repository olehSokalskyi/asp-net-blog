using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Likes;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class LikeRepository(ApplicationDbContext context) : ILikeRepository, ILikeQueries
{
    public async Task<IReadOnlyList<Like>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Likes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Option<Like>> GetById(LikeId id, CancellationToken cancellationToken)
    {
        var entity = await context.Likes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Like>() : Option.Some(entity);
    }
    
    public async Task<Like> Add(Like like, CancellationToken cancellationToken)
    {
        await context.Likes.AddAsync(like, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return like;
    }
    
    public async Task<Like> Delete(Like like, CancellationToken cancellationToken)
    {
        context.Likes.Remove(like);

        await context.SaveChangesAsync(cancellationToken);

        return like;
    }
}