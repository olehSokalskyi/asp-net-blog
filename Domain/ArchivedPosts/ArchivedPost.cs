using Domain.Posts;

namespace Domain.ArchivedPosts;

public class ArchivedPost
{
    public ArchivedPostId Id { get; }
    
    public DateTime ArchivedAt { get; private set; }
    
    public PostId PostId { get; }
    public Post? Post { get; }
    
    private ArchivedPost(ArchivedPostId id, PostId postId, DateTime archivedAt)
    {
        Id = id;
        PostId = postId;
        ArchivedAt = archivedAt;
    }
    
    public static ArchivedPost New(ArchivedPostId id, PostId postId)
        => new(id, postId, DateTime.UtcNow);
}