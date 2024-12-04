using Domain.Posts;
using Domain.Users;

namespace Domain.Comments;

public class Comment
{
    public CommentId Id { get; }
    public string Body { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public UserId UserId { get; }
    public User? User { get; }

    public PostId PostId { get; }
    public Post? Post { get; }

    public Comment(CommentId id, string body, DateTime createdAt, DateTime updatedAt, UserId userId, PostId postId)
    {
        Id = id;
        Body = body;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
        PostId = postId;
    }

    public static Comment New(CommentId id, string body, UserId userId, PostId postId)
        => new(id, body, DateTime.UtcNow, DateTime.UtcNow, userId, postId);

    public void UpdateDetails(string body)
    {
        Body = body;
        UpdatedAt = DateTime.UtcNow;
    }
}