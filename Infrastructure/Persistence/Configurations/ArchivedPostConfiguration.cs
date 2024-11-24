using Domain.ArchivedPosts;
using Domain.Likes;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ArchivedPostConfiguration : IEntityTypeConfiguration<ArchivedPost>
{
    public void Configure(EntityTypeBuilder<ArchivedPost> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new ArchivedPostId(x));
       
        builder.Property(x => x.ArchivedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        builder.HasOne(x => x.Post)
            .WithMany(x => x.ArchivedPosts)
            .HasForeignKey(x => x.PostId);
    }
}