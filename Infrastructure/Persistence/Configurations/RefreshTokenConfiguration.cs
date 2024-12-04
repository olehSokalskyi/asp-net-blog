using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new RefreshTokenId(x));
        builder.Property(x => x.Token).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Expires).HasConversion(new DateTimeUtcConverter());
        builder.Property(x => x.CreatedAt).HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        builder.Property(x => x.RevokedAt).HasConversion(new DateTimeUtcConverter());
        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_refresh_tokens_users_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}