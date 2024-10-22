using Domain.Chats;
using Domain.Users;

namespace Domain.Messages;

public class Message
{
    public MessageId Id { get; }
    public ChatId ChatId { get; }
    public Chat Chat { get; }
    public UserId UserId { get; }
    public User User { get; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private Message(MessageId id, ChatId chatId, UserId userId, string content, DateTime createdAt)
    {
        Id = id;
        ChatId = chatId;
        UserId = userId;
        Content = content;
        CreatedAt = createdAt;
    }
    
    public static Message New(MessageId id, ChatId chatId, UserId userId, string content)
        => new(id, chatId, userId, content, DateTime.UtcNow);
    
    
}