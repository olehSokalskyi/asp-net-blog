using Domain.ArchivedPosts;

namespace Api.Dtos;

public record ArchivedPostDto(
    Guid? Id,
    Guid PostId,
    DateTime? ArchivedAt)
{
    public static ArchivedPostDto FromDomainModel(ArchivedPost archivedPost)
        => new(
            Id: archivedPost.Id.Value,
            PostId: archivedPost.PostId.Value,
            ArchivedAt: archivedPost.ArchivedAt);
}