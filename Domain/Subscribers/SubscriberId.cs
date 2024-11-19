namespace Domain.Subscribers;

public record SubscriberId(Guid Value)
{
    public static SubscriberId New() => new(Guid.NewGuid());
    public static SubscriberId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}