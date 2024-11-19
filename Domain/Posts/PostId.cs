namespace Domain.Posts;

public record PostId(Guid Value)
{
    public static PostId New() => new(Guid.NewGuid());
    public static PostId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}