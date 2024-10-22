namespace Domain.Chats;

public record ChatId(Guid Value)
{
    public static ChatId New() => new(Guid.NewGuid());
    public static ChatId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}