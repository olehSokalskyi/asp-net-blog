namespace Domain.Likes;

public record LikeId(Guid Value)
{
    public static LikeId New() => new(Guid.NewGuid());
    public static LikeId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}