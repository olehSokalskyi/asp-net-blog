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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatQueries, ChatRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserQueries, UserRepository>();
        services.AddScoped<IRoleQueries, RoleRepository>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<ILikeQueries, LikeRepository>();
        services.AddScoped<IGenderQueries, GenderRepository>();
        services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        services.AddScoped<ISubscriberQueries, SubscriberRepository>();
        services.AddScoped<IArchivedPostRepository, ArchivedPostRepository>();
        services.AddScoped<IArchivedPostQueries, ArchivedPostRepository>();
        
        services.AddScoped<CategoryRepository>();
        services.AddScoped<ICategoryRepository>(provider => provider.GetRequiredService<CategoryRepository>());
        services.AddScoped<ICategoryQueries>(provider => provider.GetRequiredService<CategoryRepository>());
        
        services.AddScoped<PostRepository>();
        services.AddScoped<IPostRepository>(provider => provider.GetRequiredService<PostRepository>());
        services.AddScoped<IPostQueries>(provider => provider.GetRequiredService<PostRepository>());
        
        services.AddScoped<CommentRepository>();
        services.AddScoped<ICommentRepository>(provider => provider.GetRequiredService<CommentRepository>());
        services.AddScoped<ICommentQueries>(provider => provider.GetRequiredService<CommentRepository>());
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}