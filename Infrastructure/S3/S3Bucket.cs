using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Common.Interfaces;

namespace Infrastructure.S3;

public class S3Bucket(IAmazonS3 client) : IS3Bucket
{
    private readonly string _bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");
    
    public async Task<bool> Put(string key, string contentType, Stream fileStream, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType
        };

        var response = await client.PutObjectAsync(request, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}