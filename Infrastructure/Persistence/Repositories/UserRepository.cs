using Application.Common.Interfaces;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User> Add(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }
    public async Task<Option<User>> GetByUsername(string username, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    
    public async Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
}