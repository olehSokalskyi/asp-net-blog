using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure.Cache;

public static class ConfigureCache
{
    public static void AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationOptions = ConfigurationOptions.Parse(configuration.GetSection("Redis")["ConnectionString"], true);
        var redis = ConnectionMultiplexer.Connect(configurationOptions);

        services.AddSingleton<IConnectionMultiplexer>(redis);
        
        services.AddSingleton<ICache, RedisCache>();
    }
}