using Domain.Posts;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IPostRepository
{
    Task<Option<Post>> GetByPostAndUserId(PostId postId, UserId userId, CancellationToken cancellationToken);

    Task<Post> Add(Post post, CancellationToken cancellationToken);
    Task<Post> Update(Post post, CancellationToken cancellationToken);
    Task<Post> Delete(Post post, CancellationToken cancellationToken);
    Task<PostImage> AddImage(PostImage postImage, CancellationToken cancellationToken);
    Task<Option<Post>> GetById(PostId id, CancellationToken cancellationToken);
}