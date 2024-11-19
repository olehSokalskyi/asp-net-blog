using Domain.Posts;

namespace Domain.ArchivedPosts;

public class ArchivedPost
{
    public ArchivedPostId Id { get; }
    public Post Post { get; }
    public PostId PostId { get; }
    public DateTime ArchivedAt { get; private set; }
    
    private ArchivedPost(ArchivedPostId id, PostId postId, DateTime archivedAt)
    {
        Id = id;
        PostId = postId;
        ArchivedAt = archivedAt;
    }
    
    public static ArchivedPost New(ArchivedPostId id, PostId postId)
        => new(id, postId, DateTime.UtcNow);
}