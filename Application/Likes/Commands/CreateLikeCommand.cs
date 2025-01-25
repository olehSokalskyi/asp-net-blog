using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Likes.Exceptions;
using Domain.Likes;
using Domain.Posts;
using Domain.Users;
using MediatR;

namespace Application.Likes.Commands;

public record CreateLikeCommand : IRequest<Result<Like, LikeException>>
{
    public Guid UserId { get; init; }
    public Guid PostId { get; init; }
}

public class CreateLikeCommandHandler(
    ILikeRepository likeRepository,
    IUserRepository userRepository,
    IPostRepository postRepository)
    : IRequestHandler<CreateLikeCommand, Result<Like, LikeException>>
{
    public async Task<Result<Like, LikeException>> Handle(CreateLikeCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var postId = new PostId(request.PostId);

        var user = await userRepository.GetById(userId, cancellationToken);
        var post = await postRepository.GetById(postId, cancellationToken);

        return await user.Match(
            async u => await post.Match(
                async p => await CreateEntity(u.Id, p.Id, cancellationToken),
                () => Task.FromResult<Result<Like, LikeException>>(
                    new LikePostNotFoundException(postId))),
            () => Task.FromResult<Result<Like, LikeException>>(
                new LikeUserNotFoundException(userId)));
    }

    private async Task<Result<Like, LikeException>> CreateEntity(
        UserId userId,
        PostId postId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Like.New(LikeId.New(), postId, userId);

            return await likeRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new LikeUnknownException(LikeId.Empty(), exception);
        }
    }
}