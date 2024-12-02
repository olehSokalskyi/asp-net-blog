using System.Reflection;
using Domain.ArchivedPosts;
using Domain.Categories;
using Domain.Chats;
using Domain.Comments;
using Domain.Genders;
using Domain.Likes;
using Domain.Messages;
using Domain.Posts;
using Domain.Roles;
using Domain.Subscribers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostImage> PostImages { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<ArchivedPost> ArchivedPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}