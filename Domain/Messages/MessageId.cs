namespace Domain.Messages;

public record MessageId(Guid Value)
{
    public static MessageId New() => new(Guid.NewGuid());
    public static MessageId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}