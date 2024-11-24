using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Application.Common.Interfaces;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.S3;

public static class ConfigureS3
{
    public static void AddS3(this IServiceCollection services)
    {
        Env.Load();

        var awsOptions = new AWSOptions
        {
            Region = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION")),
            Credentials = new BasicAWSCredentials(
                Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
                Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
            )
        };
        
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IS3Bucket, S3Bucket>();
    }
}