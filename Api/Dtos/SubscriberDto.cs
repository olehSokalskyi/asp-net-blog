using Domain.Subscribers;

namespace Api.Dtos;

public record SubscriberDto(Guid? Id, Guid UserId, Guid FollowUserId, DateTime? CreatedAt)
{
    public static SubscriberDto FromDomainModel(Subscriber subscriber)
        => new(
            Id: subscriber.Id.Value,
            UserId: subscriber.UserId.Value,
            FollowUserId: subscriber.FollowUserId.Value,
            CreatedAt: subscriber.CreatedAt);
}