using Infrastructure.Authentication;
using Infrastructure.Cache;
using Infrastructure.Persistence;
using Infrastructure.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructure
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddAuthenticationJwt();
        services.AddS3();
        services.AddCache(configuration);
    }
}