using Domain.RefreshTokens;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    public Task<Option<RefreshToken>> GetByToken(string token, CancellationToken cancellationToken);
    public Task<Option<RefreshToken>> GetByUserId(UserId userId, CancellationToken cancellationToken);
    
    public Task<RefreshToken> Add(RefreshToken refreshToken, CancellationToken cancellationToken);
    public Task<RefreshToken> Update(RefreshToken refreshToken, CancellationToken cancellationToken);
    public Task<RefreshToken> Delete(RefreshToken refreshToken, CancellationToken cancellationToken);
}