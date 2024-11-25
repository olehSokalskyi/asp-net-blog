namespace Application.Common.Interfaces;

public interface IS3Bucket
{
    Task<bool> Put(
        string key,
        string contentType,
        Stream fileStream,
        CancellationToken cancellationToken);
}