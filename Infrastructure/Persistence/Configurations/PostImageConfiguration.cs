using Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
{
    public void Configure(EntityTypeBuilder<PostImage> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new PostImageId(x));

        builder.Property(x => x.PostId).HasConversion(x => x.Value, x => new PostId(x));
        builder.HasOne(x => x.Post)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.PostId)
            .HasConstraintName("fk_post_images_posts_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}