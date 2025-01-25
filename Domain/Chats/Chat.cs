using Domain.Messages;
using Domain.Users;

namespace Domain.Chats;

public class Chat
{
    public ChatId Id { get; }
    public string Name { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsGroup { get; private set; }
    
    public List<User> Users { get; private set; } = new();
    public List<Message> Messages { get; private set; } = new();

    private Chat(ChatId id, string name, DateTime createdAt, bool isGroup)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        IsGroup = isGroup;
    }

    public static Chat New(ChatId id, string name, bool isGroup)
        => new(id, name, DateTime.UtcNow, isGroup);

    public void UpdateDetails(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AddUser(User user)
    {
        Users.Add(user);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RemoveUser(User user)
    {
        Users.Remove(user);
        UpdatedAt = DateTime.UtcNow;
    }
}