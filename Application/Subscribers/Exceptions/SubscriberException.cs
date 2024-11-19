using Domain.Subscribers;
using Domain.Users;

namespace Application.Subscribers.Exceptions;

public abstract class SubscriberException(SubscriberId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public SubscriberId SubscriberId { get; } = id;
}

public class SubscriberNotFoundException(SubscriberId id) : SubscriberException(id, $"Subscriber under id: {id} not found");

public class SubscriberAlreadyExistsException(SubscriberId id) : SubscriberException(id, $"Subscriber already exists: {id}");

public class SubscriberUnknownException(SubscriberId id, Exception innerException)
    : SubscriberException(id, $"Unknown exception for the subscriber under id: {id}", innerException);
    
public class SubscriberUserNotFoundException(UserId userId)
    : SubscriberException(SubscriberId.Empty(), $"User under id:{userId} not found");

public class SubscriberFollowUserNotFoundException(UserId followUserId)
    : SubscriberException(SubscriberId.Empty(), $"Follow User under id:{followUserId} not found");