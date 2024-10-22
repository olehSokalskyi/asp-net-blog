﻿using Domain.Chats;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new UserId(x));
        builder.Property(x => x.Username).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        builder.Property(x=> x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        builder.HasMany(x => x.Chats)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "ChatUser",
                j => j.HasOne<Chat>().WithMany().HasForeignKey("ChatId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));
    }
}