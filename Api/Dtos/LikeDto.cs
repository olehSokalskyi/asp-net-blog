using Domain.Likes;

namespace Api.Dtos;

public record LikeDto(Guid? Id, Guid? UserId, Guid? PostId, DateTime? CreatedAt)
{
    public static LikeDto FromDomainModel(Like like)
        => new(
            Id: like.Id.Value,
            UserId: like.UserId.Value,
            PostId: like.PostId.Value,
            CreatedAt: like.CreatedAt);
}