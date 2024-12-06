using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authentication;

public static class ConfigureAuthentication
{
    public static void AddAuthenticationJwt(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IJwtDecoder, JwtDecoder>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }
}