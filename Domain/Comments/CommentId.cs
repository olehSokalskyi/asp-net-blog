namespace Domain.Comments;

public record CommentId(Guid Value)
{
    public static CommentId New() => new(Guid.NewGuid());
    public static CommentId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}