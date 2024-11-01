using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;

    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is ClaimsIdentity identity)
        {
            var roleClaim = identity.FindFirst(ClaimTypes.Role);
            if (roleClaim != null)
            {
                var roles = roleClaim.Value.Split(',');
                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
        }

        await _next(context);
    }
}