using Application.Common.Interfaces.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context): IRefreshTokenRepository
{
    public async Task<Option<RefreshToken>> GetByToken(string token, CancellationToken cancellationToken)
    {
        var entity = await context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        
        return entity == null ? Option.None<RefreshToken>() : Option.Some(entity);
    }

    public async Task<Option<RefreshToken>> GetByUserId(UserId userId, CancellationToken cancellationToken)
    {
        var entity = await context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        return entity == null ? Option.None<RefreshToken>() : Option.Some(entity);
    }

    public async Task<RefreshToken> Add(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }

    public async  Task<RefreshToken> Update(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        context.RefreshTokens.Update(refreshToken);
        
        await context.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }

    public async Task<RefreshToken> Delete(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        context.RefreshTokens.Remove(refreshToken);
        
        await context.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }
}