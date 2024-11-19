using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Posts.Exceptions;
using Domain.Posts;
using Domain.Users;
using MediatR;

namespace Application.Posts.Commands;

public record CreatePostCommand : IRequest<Result<Post, PostException>>
{
    public required string Body { get; init; }
    public required Guid UserId { get; init; }
}

public class CreatePostCommandHandler(
    IPostRepository postRepository,
    IUserRepository userRepository) : IRequestHandler<CreatePostCommand, Result<Post, PostException>>
{
    public async Task<Result<Post, PostException>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);

        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match(
            async u => await CreateEntity(request.Body, userId, cancellationToken),
            () => Task.FromResult<Result<Post, PostException>>(new PostUserNotFoundException(PostId.Empty(), userId)));
    }

    private async Task<Result<Post, PostException>> CreateEntity(
        string body,
        UserId userId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Post.New(PostId.New(), body, userId);

            return await postRepository.Add(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new PostUnknownException(PostId.Empty(), e);
        }
    }
}