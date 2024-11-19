using Domain.Chats;
using Domain.Genders;
using Domain.Likes;
using Domain.Messages;
using Domain.Roles;
using Domain.Subscribers;

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
    public List<Like> Likes { get; private set; } = new();
    public RoleId RoleId { get; private set;}
    public Role? Role { get; }
    public GenderId GenderId { get; private set; }
    public Gender? Gender { get; }
    public List<Subscriber> Subscribers { get; private set; } = new();
    public List<Subscriber> Followers { get; private set; } = new();
     

    private User(UserId id, string username, string firstName, string lastName, string email, string password,
        DateTime createdAt, string profilePicture, RoleId roleId, GenderId genderId)
    {
        Id = id;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        ProfilePicture = profilePicture;
        RoleId = roleId;
        GenderId = genderId;
    }

    public static User New(UserId id, string username, string firstName, string lastName, string email, string password,
        string profilePicture, RoleId roleId, GenderId genderId)
        => new(id, username, firstName, lastName, email, password, DateTime.UtcNow, profilePicture, roleId, genderId);

    public void UpdateDetails(string username, string firstName, string lastName, GenderId genderId)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow; 
        GenderId = genderId;
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
    
    public void UpdateRole(RoleId role)
    {
        RoleId = role;
    }
}