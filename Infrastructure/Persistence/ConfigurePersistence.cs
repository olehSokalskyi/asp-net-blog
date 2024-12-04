using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Converters;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();
        var dataSource = dataSourceBuild.Build();

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(
                    dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
        services.AddServices();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository>(provider => provider.GetRequiredService<IUserRepository>());
        services.AddScoped<IUserQueries>(provider => provider.GetRequiredService<UserRepository>());
        
        services.AddScoped<IChatRepository, ChatRepository>();
        
        services.AddScoped<ChatRepository>();
        services.AddScoped<IChatRepository>(provider => provider.GetRequiredService<ChatRepository>());
        
        services.AddScoped<IMessageRepository, MessageRepository>();
        
        services.AddScoped<MessageRepository>();
        services.AddScoped<IMessageRepository>(provider => provider.GetRequiredService<MessageRepository>());
        
        services.AddScoped<RoleRepository>();
        services.AddScoped<IRoleRepository>(provider => provider.GetRequiredService<RoleRepository>());
        services.AddScoped<IRoleQueries>(provider => provider.GetRequiredService<RoleRepository>());
        
        services.AddScoped<GenderRepository>();
        services.AddScoped<IGenderRepository>(provider => provider.GetRequiredService<GenderRepository>());
        services.AddScoped<IGenderQueries>(provider => provider.GetRequiredService<GenderRepository>());
        
        services.AddScoped<LikeRepository>();
        services.AddScoped<ILikeRepository>(provider => provider.GetRequiredService<LikeRepository>());
        services.AddScoped<ILikeQueries>(provider => provider.GetRequiredService<LikeRepository>());
        
        services.AddScoped<SubscriberRepository>();
        services.AddScoped<ISubscriberRepository>(provider => provider.GetRequiredService<SubscriberRepository>());
        services.AddScoped<ISubscriberQueries>(provider => provider.GetRequiredService<SubscriberRepository>());
        
        services.AddScoped<ArchivedPostRepository>();
        services.AddScoped<IArchivedPostRepository>(provider => provider.GetRequiredService<ArchivedPostRepository>());
        services.AddScoped<IArchivedPostQueries>(provider => provider.GetRequiredService<ArchivedPostRepository>());
        
        services.AddScoped<CategoryRepository>();
        services.AddScoped<ICategoryRepository>(provider => provider.GetRequiredService<CategoryRepository>());
        services.AddScoped<ICategoryQueries>(provider => provider.GetRequiredService<CategoryRepository>());
        
        services.AddScoped<PostRepository>();
        services.AddScoped<IPostRepository>(provider => provider.GetRequiredService<PostRepository>());
        services.AddScoped<IPostQueries>(provider => provider.GetRequiredService<PostRepository>());
        
        services.AddScoped<CommentRepository>();
        services.AddScoped<ICommentRepository>(provider => provider.GetRequiredService<CommentRepository>());
        services.AddScoped<ICommentQueries>(provider => provider.GetRequiredService<CommentRepository>());
        
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}