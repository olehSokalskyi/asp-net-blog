using Domain.ArchivedPosts;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ArchivedPostRepository(ApplicationDbContext context) : IArchivedPostRepository, IArchivedPostQueries
{
    public async Task<IReadOnlyList<ArchivedPost>> GetAll(CancellationToken cancellationToken)
    {
        return await context.ArchivedPosts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<ArchivedPost>> GetById(ArchivedPostId id, CancellationToken cancellationToken)
    {
        var entity = await context.ArchivedPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return entity == null ? Option.None<ArchivedPost>() : Option.Some(entity);
    }

    public async Task<ArchivedPost> Add(ArchivedPost archivedPost, CancellationToken cancellationToken)
    {
        await context.ArchivedPosts.AddAsync(archivedPost, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return archivedPost;
    }

    public async Task<ArchivedPost> Delete(ArchivedPost archivedPost, CancellationToken cancellationToken)
    {
        context.ArchivedPosts.Remove(archivedPost);

        await context.SaveChangesAsync(cancellationToken);

        return archivedPost;
    }
}