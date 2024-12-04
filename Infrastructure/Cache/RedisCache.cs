using Application.Common.Extensions;
using Application.Common.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Cache;

public class RedisCache(IConnectionMultiplexer connectionMultiplexer) : ICache
{
    private readonly TimeSpan _expiration = TimeSpan.FromMinutes(10);
    
    public async Task Set<T>(string key, T value)
    {
        await Set(key, value, _expiration);
    }

    public async Task Set<T>(string key, T value, TimeSpan expiration)
    {
        var db = connectionMultiplexer.GetDatabase();
        var json = value.Serialize();
        await db.StringSetAsync(key, json, expiration);
    }

    public async Task<T?> Get<T>(string key)
    {
        var db = connectionMultiplexer.GetDatabase();
        var json = await db.StringGetAsync(key);
        if (json.IsNullOrEmpty)
        {
            return default;
        }

        return json.ToString().Deserialize<T>();
    }
    
    public async Task<bool> Delete(string key)
    {
        var db = connectionMultiplexer.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }
}