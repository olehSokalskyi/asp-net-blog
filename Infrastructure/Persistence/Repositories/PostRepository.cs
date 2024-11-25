using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Posts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class PostRepository(ApplicationDbContext context) : IPostRepository, IPostQueries
{
    public async Task<IReadOnlyList<Post>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Posts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Post>> GetById(PostId id, CancellationToken cancellationToken)
    {
        var entity = await context.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Post>() : Option.Some(entity);
    }

    public async Task<Option<Post>> GetByPostAndUserId(
        PostId postId,
        UserId userId,
        CancellationToken cancellationToken)
    {
        var entity = await context.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == postId && x.UserId == userId, cancellationToken);

        return entity == null ? Option.None<Post>() : Option.Some(entity);
    }

    public async Task<Post> Add(Post post, CancellationToken cancellationToken)
    {
        await context.Posts.AddAsync(post, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return post;
    }

    public async Task<Post> Update(Post post, CancellationToken cancellationToken)
    {
        context.Posts.Update(post);

        await context.SaveChangesAsync(cancellationToken);

        return post;
    }

    public async Task<Post> Delete(Post post, CancellationToken cancellationToken)
    {
        context.Posts.Remove(post);

        await context.SaveChangesAsync(cancellationToken);

        return post;
    }

    public async Task<PostImage> AddImage(PostImage postImage, CancellationToken cancellationToken)
    {
        await context.PostImages.AddAsync(postImage, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return postImage;
    }
}