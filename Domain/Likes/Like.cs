using Domain.Posts;
using Domain.Users;

namespace Domain.Likes;

public class Like
{
    public LikeId Id { get; }
    
    public DateTime CreatedAt { get; private set; }
    
    public PostId PostId { get; }
    public Post? Post { get; }
    
    public UserId UserId { get; }
    public User? User { get; }
    
    private Like(LikeId id, PostId postId, UserId userId, DateTime createdAt)
    {
        Id = id;
        PostId = postId;
        UserId = userId;
        CreatedAt = createdAt;
    }
    
    public static Like New(LikeId id, PostId postId, UserId userId)
        => new(id, postId, userId, DateTime.UtcNow);
}