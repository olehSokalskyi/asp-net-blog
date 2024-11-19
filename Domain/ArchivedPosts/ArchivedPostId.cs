namespace Domain.ArchivedPosts;

public record ArchivedPostId(Guid Value)
{
    public static ArchivedPostId New() => new(Guid.NewGuid());
    public static ArchivedPostId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}