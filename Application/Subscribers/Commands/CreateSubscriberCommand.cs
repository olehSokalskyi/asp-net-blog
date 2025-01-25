using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Subscribers.Exceptions;
using Domain.Subscribers;
using Domain.Users;
using MediatR;

namespace Application.Subscribers.Commands;

public record CreateSubscriberCommand : IRequest<Result<Subscriber, SubscriberException>>
{
    public Guid UserId { get; init; }
    public Guid FollowUserId { get; init; }
}

public class CreateSubscriberCommandHandler(
    ISubscriberRepository subscriberRepository,
    IUserRepository userRepository)
    : IRequestHandler<CreateSubscriberCommand, Result<Subscriber, SubscriberException>>
{
    public async Task<Result<Subscriber, SubscriberException>> Handle(
        CreateSubscriberCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var followUserId = new UserId(request.FollowUserId);

        var user = await userRepository.GetById(userId, cancellationToken);
        var followUser = await userRepository.GetById(followUserId, cancellationToken);

        return await user.Match(
            async u => await followUser.Match(
                async f => await CreateEntity(u.Id, f.Id, cancellationToken),
                () => Task.FromResult<Result<Subscriber, SubscriberException>>(
                    new SubscriberFollowUserNotFoundException(followUserId))),
            () => Task.FromResult<Result<Subscriber, SubscriberException>>(
                new SubscriberUserNotFoundException(userId)));
    }

    private async Task<Result<Subscriber, SubscriberException>> CreateEntity(
        UserId userId,
        UserId followUserId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Subscriber.New(SubscriberId.New(), followUserId, userId);

            return await subscriberRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new SubscriberUnknownException(SubscriberId.Empty(), exception);
        }
    }
}