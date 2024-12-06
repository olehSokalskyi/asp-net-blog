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
    public UserId ChatOwnerId { get; private set; }
    public User ChatOwner { get; private set; }
    public List<Message> Messages { get; private set; } = new();

    private Chat() { }
    private Chat(ChatId id, string name, DateTime createdAt, bool isGroup, UserId chatOwnerId, List<User> users)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        IsGroup = isGroup;
        ChatOwnerId = chatOwnerId;
        Users = users;
    }



    public static Chat New(ChatId id, string name, bool isGroup, UserId chatOwnerId, List<User> users)
        => new(id, name, DateTime.UtcNow, isGroup, chatOwnerId, users);

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