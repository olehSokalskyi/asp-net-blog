using Domain.Comments;

namespace Api.Dtos;

public record CommentDto(
    Guid? Id,
    string? Body,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? UserId,
    UserDto? User,
    Guid? PostId,
    PostDto? Post)
{
    public static CommentDto FromDomainModel(Comment comment)
        => new(
            Id: comment.Id.Value,
            Body: comment.Body,
            CreatedAt: comment.CreatedAt,
            UpdatedAt: comment.UpdatedAt,
            UserId: comment.UserId.Value,
            User: comment.User is null ? null : UserDto.FromDomainModel(comment.User),
            PostId: comment.PostId.Value,
            Post: comment.Post is null ? null : PostDto.FromDomainModel(comment.Post));
}