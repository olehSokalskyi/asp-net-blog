using Domain.Subscribers;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ISubscriberRepository
{
    Task<Option<Subscriber>> GetById(SubscriberId id, CancellationToken cancellationToken);
    
    Task<Subscriber> Add(Subscriber subscriber, CancellationToken cancellationToken);
    Task<Subscriber> Delete(Subscriber subscriber, CancellationToken cancellationToken);
}