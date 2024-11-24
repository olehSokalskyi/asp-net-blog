using Domain.Users;

namespace Domain.Subscribers;

public class Subscriber
{
    public SubscriberId Id { get; }
    public UserId UserId { get; }
    public User User { get; }
    public UserId FollowUserId { get; }
    public User FollowUser { get; }
    public DateTime CreatedAt { get; private set; }
    
    private Subscriber(SubscriberId id, UserId followUserId, UserId userId, DateTime createdAt)
    {
        Id = id;
        FollowUserId = followUserId;
        UserId = userId;
        CreatedAt = createdAt;
    }
    
    public static Subscriber New(SubscriberId id, UserId followUserId, UserId userId)
        => new(id, followUserId, userId, DateTime.UtcNow);
}