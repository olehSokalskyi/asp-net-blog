using Application.Common.Interfaces;
using Domain.Genders;
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
    }
}