namespace Application.Common.Interfaces;

public interface ICache
{
    Task Set<T>(string key, T value);
    Task Set<T>(string key, T value, TimeSpan expiration);
    Task<T?> Get<T>(string key);
    Task<bool> Delete(string key);
}