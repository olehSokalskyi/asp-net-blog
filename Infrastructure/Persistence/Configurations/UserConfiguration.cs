using Domain.Chats;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.Username).IsRequired().HasColumnType("varchar(255)");

        builder.Property(x => x.FirstName).HasColumnType("varchar(255)");

        builder.Property(x => x.LastName).HasColumnType("varchar(255)");

        builder.Property(x => x.Email).IsRequired().HasColumnType("varchar(255)");

        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Password).IsRequired().HasColumnType("varchar(255)");

        builder.Property(x => x.ProfilePicture).HasColumnType("varchar(255)");

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");

        builder.HasMany(x => x.Chats)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "ChatUser",
                j => j.HasOne<Chat>().WithMany().HasForeignKey("ChatId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .HasConstraintName("fk_users_roles_id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Gender)
            .WithMany()
            .HasForeignKey(x => x.GenderId)
            .HasConstraintName("fk_users_genders_id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_refresh_tokens_users_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}