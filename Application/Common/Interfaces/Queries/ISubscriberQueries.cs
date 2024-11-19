using Domain.Subscribers;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ISubscriberQueries
{
    Task<IReadOnlyList<Subscriber>> GetAll(CancellationToken cancellationToken);
    Task<Option<Subscriber>> GetById(SubscriberId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Subscriber>> GetByUserId(UserId userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Subscriber>> GetByFollowUserId(UserId followUserId, CancellationToken cancellationToken);

}