using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository, IUserQueries
{
    public async Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .Include(x=> x.Role)
            .ToListAsync(cancellationToken);
    }
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
            .Include(x=> x.Role)
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    
    public async Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .AsNoTracking()
            .Include(x=> x.Role)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    
    public async Task<Option<User>> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .AsNoTracking()
            .Include(x=> x.Role)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        return entity == null ? Option.None<User>() : Option.Some(entity);
    }
    
    public async Task<User> Update(User user, CancellationToken cancellationToken)
    {
        context.Users.Update(user);

        await context.SaveChangesAsync(cancellationToken);

        return user;
    }
    public async Task<User> Delete(User user, CancellationToken cancellationToken)
    {
        context.Users.Remove(user);

        await context.SaveChangesAsync(cancellationToken);

        return user;
    }
}