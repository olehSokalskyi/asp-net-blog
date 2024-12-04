using Domain.Subscribers;

namespace Api.Dtos;

public record SubscriberDto(Guid? Id, Guid UserId, Guid? FollowUserId, UserDto? FollowUser, DateTime? CreatedAt)
{
    public static SubscriberDto FromDomainModel(Subscriber subscriber)
        => new(
            Id: subscriber.Id.Value,
            UserId: subscriber.UserId.Value,
            FollowUserId: subscriber.FollowUserId.Value,
            FollowUser: subscriber.FollowUser is null ? null : UserDto.FromDomainModel(subscriber.FollowUser),
            CreatedAt: subscriber.CreatedAt);
}