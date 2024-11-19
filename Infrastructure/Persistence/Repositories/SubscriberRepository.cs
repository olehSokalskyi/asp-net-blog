using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Subscribers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class SubscriberRepository(ApplicationDbContext context) : ISubscriberRepository, ISubscriberQueries
{
    public async Task<IReadOnlyList<Subscriber>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Subscribers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Option<Subscriber>> GetById(SubscriberId id, CancellationToken cancellationToken)
    {
        var entity = await context.Subscribers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Subscriber>() : Option.Some(entity);
    }
    
    public async Task<IReadOnlyList<Subscriber>> GetByUserId(UserId userId, CancellationToken cancellationToken)
    {
        return await context.Subscribers
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Subscriber>> GetByFollowUserId(UserId followUserId, CancellationToken cancellationToken)
    {
        return await context.Subscribers
            .AsNoTracking()
            .Where(s => s.FollowUserId == followUserId)
            .ToListAsync(cancellationToken);
    }

    
    public async Task<Subscriber> Add(Subscriber subscriber, CancellationToken cancellationToken)
    {
        await context.Subscribers.AddAsync(subscriber, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return subscriber;
    }
    
    public async Task<Subscriber> Delete(Subscriber subscriber, CancellationToken cancellationToken)
    {
        context.Subscribers.Remove(subscriber);

        await context.SaveChangesAsync(cancellationToken);

        return subscriber;
    }
}