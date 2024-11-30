using Domain.Posts;

namespace Api.Dtos;

public record PostImageDto(Guid Id, string Url)
{
    public static PostImageDto FromDomainModel(PostImage postImage)
        => new(Id: postImage.Id.Value, Url: $"{Environment.GetEnvironmentVariable("AWS_URL")}/{postImage.ImagePath}");
}