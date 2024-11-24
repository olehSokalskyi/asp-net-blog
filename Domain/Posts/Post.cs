using Domain.ArchivedPosts;
using Domain.Likes;
using Domain.Users;

namespace Domain.Posts;

public class Post
{
    public PostId Id { get; }
    public string Body { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public UserId UserId { get; }
    public User? User { get; }
    
    public List<Like> Likes { get; private set; } = new();
    public List<ArchivedPost> ArchivedPosts { get; private set; } = new();
    
    private Post(PostId id, string body, DateTime createdAt, DateTime updatedAt, UserId userId)
    {
        Id = id;
        Body = body;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }

    public static Post New(PostId id, string body, UserId userId)
        => new(id, body, DateTime.UtcNow, DateTime.UtcNow, userId);

    public void UpdateDetails(string body)
    {
        Body = body;
        UpdatedAt = DateTime.UtcNow;
    }
}