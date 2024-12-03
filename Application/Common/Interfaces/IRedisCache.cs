namespace Infrastructure.RedisCache;

public interface IRedisCache
{
    public Task<(Guid UserId, string ChatConnectionId)> GetAsync(Guid key);
    public Task SetAsync(Guid UserId, string ChatConnectionId);
    public Task RemoveAsync(Guid key);
}