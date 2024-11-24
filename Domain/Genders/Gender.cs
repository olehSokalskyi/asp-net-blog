using Domain.Users;

namespace Domain.Genders;

public class Gender
{
    public GenderId Id { get; }
    public string Title { get; private set; }

    private Gender(GenderId id, string title)
    {
        Id = id;
        Title = title;
    }

    public static Gender New(GenderId id, string title)
        => new(id, title);

    public void UpdateDetails(string title)
    {
        Title = title;
    }
}