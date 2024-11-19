using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Subscribers.Exceptions;
using Domain.Subscribers;
using MediatR;

namespace Application.Subscribers.Commands;

public record DeleteSubscriberCommand : IRequest<Result<Subscriber, SubscriberException>>
{
    public required Guid SubscriberId { get; init; }
}

public class DeleteSubscriberCommandHandler(ISubscriberRepository subscriberRepository)
    : IRequestHandler<DeleteSubscriberCommand, Result<Subscriber, SubscriberException>>
{
    public async Task<Result<Subscriber, SubscriberException>> Handle(
        DeleteSubscriberCommand request,
        CancellationToken cancellationToken)
    {
        var subscriberId = new SubscriberId(request.SubscriberId);

        var existingSubscriber = await subscriberRepository.GetById(subscriberId, cancellationToken);

        return await existingSubscriber.Match<Task<Result<Subscriber, SubscriberException>>>(
            async s => await DeleteEntity(s, cancellationToken),
            () => Task.FromResult<Result<Subscriber, SubscriberException>>(new SubscriberNotFoundException(subscriberId)));
    }

    public async Task<Result<Subscriber, SubscriberException>> DeleteEntity(Subscriber subscriber, CancellationToken cancellationToken)
    {
        try
        {
            return await subscriberRepository.Delete(subscriber, cancellationToken);
        }
        catch (Exception exception)
        {
            return new SubscriberUnknownException(subscriber.Id, exception);
        }
    }
}