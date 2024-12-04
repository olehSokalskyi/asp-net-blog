using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Comments;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository, ICommentQueries
{
    public async Task<IReadOnlyList<Comment>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Comments
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Post)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Comment>> GetById(CommentId id, CancellationToken cancellationToken)
    {
        var entity = await context.Comments
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Post)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Comment>() : Option.Some(entity);
    }

    public async Task<Option<Comment>> GetByCommentAndUserId(
        CommentId commentId,
        UserId userId,
        CancellationToken cancellationToken)
    {
        var entity = await context.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == commentId && x.UserId == userId, cancellationToken);

        return entity == null ? Option.None<Comment>() : Option.Some(entity);
    }

    public async Task<Comment> Add(Comment comment, CancellationToken cancellationToken)
    {
        await context.Comments.AddAsync(comment, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return comment;
    }

    public async Task<Comment> Update(Comment comment, CancellationToken cancellationToken)
    {
        context.Comments.Update(comment);

        await context.SaveChangesAsync(cancellationToken);

        return comment;
    }

    public async Task<Comment> Delete(Comment comment, CancellationToken cancellationToken)
    {
        context.Comments.Remove(comment);

        await context.SaveChangesAsync(cancellationToken);

        return comment;
    }
}