namespace Domain.Genders;

public record GenderId(Guid Value)
{
    public static GenderId New() => new(Guid.NewGuid());
    public static GenderId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}