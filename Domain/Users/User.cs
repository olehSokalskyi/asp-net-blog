﻿using Domain.Chats;
using Domain.Messages;
using Domain.Roles;

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
    public RoleId RoleId { get; private set;}
    public Role? Role { get; }

    private User(UserId id, string username, string firstName, string lastName, string email, string password,
        DateTime createdAt, string profilePicture, RoleId roleId)
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
    }

    public static User New(UserId id, string username, string firstName, string lastName, string email, string password,
        string profilePicture, RoleId roleId)
        => new(id, username, firstName, lastName, email, password, DateTime.UtcNow, profilePicture, roleId);

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
    
    public void ChangeRole(RoleId role)
    {
        RoleId = role;
    }
}