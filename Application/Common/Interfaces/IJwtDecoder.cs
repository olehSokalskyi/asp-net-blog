using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface IJwtDecoder
{
    IEnumerable<Claim> DecodeToken(string token);
}