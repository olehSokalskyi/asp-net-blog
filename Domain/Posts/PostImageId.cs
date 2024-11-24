namespace Domain.Posts;

public record PostImageId(Guid Value)
{
    public static PostImageId New() => new(Guid.NewGuid());
    public static PostImageId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}