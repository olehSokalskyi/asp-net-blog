using Domain.Chats;
using Domain.Messages;

namespace Domain.Users;

public class User
{
    public UserId Id { get; }
    public string Username { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Chat> Chats { get; private set; } = new();
    public List<Message> Messages { get; private set; } = new();

    private User(UserId id, string username, DateTime createdAt)
    {
        Id = id;
        Username = username;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static User New(UserId id, string username)
        => new(id, username, DateTime.UtcNow);

    public void UpdateDetails(string username)
    {
        Username = username;
        UpdatedAt = DateTime.UtcNow;
    }
}