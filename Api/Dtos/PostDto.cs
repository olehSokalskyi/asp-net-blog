using Domain.Posts;

namespace Api.Dtos;

public record PostDto(
    Guid? Id,
    string? Body,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? UserId,
    IFormFile? File)
{
    public static PostDto FromDomainModel(Post post)
        => new(
            Id: post.Id.Value,
            Body: post.Body,
            CreatedAt: post.CreatedAt,
            UpdatedAt: post.UpdatedAt,
            UserId: post.UserId.Value,
            File: null);
}