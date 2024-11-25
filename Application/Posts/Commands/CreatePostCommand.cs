using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Posts.Exceptions;
using Domain.Posts;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands;

public record CreatePostCommand : IRequest<Result<Post, PostException>>
{
    public required string Body { get; init; }
    public required Guid UserId { get; init; }
    public required IFormFile File { get; init; }
}

public class CreatePostCommandHandler(
    IPostRepository postRepository,
    IUserRepository userRepository,
    IS3Bucket s3) : IRequestHandler<CreatePostCommand, Result<Post, PostException>>
{
    public async Task<Result<Post, PostException>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);

        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match(
            async _ => await CreateEntity(request.Body, userId, request.File, cancellationToken),
            () => Task.FromResult<Result<Post, PostException>>(new PostUserNotFoundException(PostId.Empty(), userId)));
    }

    private async Task<Result<Post, PostException>> CreateEntity(
        string body,
        UserId userId,
        IFormFile formFile,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Post.New(PostId.New(), body, userId);

            var post = await postRepository.Add(entity, cancellationToken);

            return await CreateImageEntity(post, formFile, cancellationToken);
        }
        catch (Exception e)
        {
            return new PostUnknownException(PostId.Empty(), e);
        }
    }

    private async Task<Result<Post, PostException>> CreateImageEntity(
        Post post,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        try
        {
            var fileStream = file.OpenReadStream();

            var memoryStream = new MemoryStream();

            await fileStream.CopyToAsync(memoryStream, cancellationToken);
            await memoryStream.FlushAsync(cancellationToken);

            var entity = PostImage.New(PostImageId.New(), post.Id);

            var result = await s3.Put(entity.ImagePath, file.ContentType, memoryStream, cancellationToken);
            if (!result)
            {
                return new PostFailedToUploadImage(post.Id);
            }
            
            entity = await postRepository.AddImage(entity, cancellationToken);

            return post;
        }
        catch (Exception e)
        {
            Console.WriteLine("there");
            Console.WriteLine(e.StackTrace);
            return new PostUnknownException(post.Id, e);
        }
    }
}