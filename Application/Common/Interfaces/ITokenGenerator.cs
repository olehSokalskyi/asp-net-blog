using Domain.Users;

namespace Application.Common.Interfaces;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}