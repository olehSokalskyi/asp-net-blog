using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Interfaces;
using Domain.Users;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class TokenGenerator: ITokenGenerator
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = "LifeIsGoodLifeIsGoodLifeIsGoodLifeIsGoodLifeIsGoodLifeIsGoodLifeIsGoodLifeIsGood"u8.ToArray();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new("role", user.Role.Name),
            new(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "https://localhost:5001",
            Audience = "https://localhost:5001",
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}