using Domain.Chats;
using Domain.Messages;

namespace Domain.Users;

public class User
{
    public UserId Id { get; }
    public string Username { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string ProfilePicture { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Chat> Chats { get; private set; } = new();
    public List<Message> Messages { get; private set; } = new();

    private User(UserId id, string username, string firstName, string lastName, string email, string password,
        DateTime createdAt)
    {
        Id = id;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static User New(UserId id, string username, string firstName, string lastName, string email, string password)
        => new(id, username, firstName, lastName, email, password, DateTime.UtcNow);

    public void UpdateDetails(string username, string firstName, string lastName)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfilePicture(string profilePicture)
    {
        ProfilePicture = profilePicture;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string password)
    {
        Password = password;
        UpdatedAt = DateTime.UtcNow;
    }
}