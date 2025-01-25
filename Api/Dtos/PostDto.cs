using Domain.Posts;

namespace Api.Dtos;

public record PostDto(
    Guid? Id,
    string? Body,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? UserId,
    UserDto? User,
    IFormFile? File,
    IReadOnlyList<PostImageDto?>? Images)
{
    public static PostDto FromDomainModel(Post post)
        => new(
            Id: post.Id.Value,
            Body: post.Body,
            CreatedAt: post.CreatedAt,
            UpdatedAt: post.UpdatedAt,
            UserId: post.UserId.Value,
            User: post.User is null ? null : UserDto.FromDomainModel(post.User),
            File: null,
            Images: post.Images?.Select(PostImageDto.FromDomainModel).ToList());
}