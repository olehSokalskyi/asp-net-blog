using Application.Common.Interfaces;
using Domain.Chats;
using Domain.Genders;
using Domain.Messages;
using Domain.Roles;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(ApplicationDbContext context)
{
    private PasswordHasher passwordHasher;
    public async Task InitializeAsync()
    {
        passwordHasher = new PasswordHasher();
        await context.Database.MigrateAsync();
        await SeedAsync();
    }

    private async Task SeedAsync()
{
    if (!context.Roles.Any())
    {
        var adminRole = Role.New(new RoleId(Guid.NewGuid()), "Admin");
        var userRole = Role.New(new RoleId(Guid.NewGuid()), "User");

        context.Roles.AddRange(adminRole, userRole);
        await context.SaveChangesAsync();
    }

    if (!context.Genders.Any())
    {
        var manGender = Gender.New(new GenderId(Guid.NewGuid()), "Man");
        var womanGender = Gender.New(new GenderId(Guid.NewGuid()), "Woman");

        context.Genders.AddRange(manGender, womanGender);
        await context.SaveChangesAsync();
    }

    if (!context.Users.Any())
    {
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole != null)
        {
            var passwordHash = passwordHasher.HashPassword("adminAdmin123");
            var adminUser = User.New(
                new UserId(Guid.NewGuid()),
                "admin",
                "Admin",
                "User",
                "admin@example.com",
                passwordHash,
                "profilePictureUrl",
                adminRole.Id);
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }

    if (!context.Chats.Any())
    {
        var users = await context.Users.ToListAsync();
        if (users.Count > 0)
        {
            for (int i = 1; i <= 3; i++)
            {
                var chat = Chat.New(
                    new ChatId(Guid.NewGuid()),
                    $"Chat {i}",
                    false,
                    users[0].Id,
                    users);

                context.Chats.Add(chat);
                await context.SaveChangesAsync();

                for (int j = 1; j <= 3; j++)
                {
                    var message = Message.New(
                        new MessageId(Guid.NewGuid()),
                        chat.Id,
                        users[0].Id,
                        $"Message {j} in Chat {i}");

                    context.Messages.Add(message);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
}