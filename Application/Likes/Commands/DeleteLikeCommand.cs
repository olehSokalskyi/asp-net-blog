using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Likes.Exceptions;
using Domain.Likes;
using MediatR;

namespace Application.Likes.Commands;

public record DeleteLikeCommand : IRequest<Result<Like, LikeException>>
{
    public required Guid LikeId { get; init; }
}

public class DeleteLikeCommandHandler(ILikeRepository likeRepository)
    : IRequestHandler<DeleteLikeCommand, Result<Like, LikeException>>
{
    public async Task<Result<Like, LikeException>> Handle(
        DeleteLikeCommand request,
        CancellationToken cancellationToken)
    {
        var likeId = new LikeId(request.LikeId);

        var existingLike = await likeRepository.GetById(likeId, cancellationToken);

        return await existingLike.Match<Task<Result<Like, LikeException>>>(
            async l => await DeleteEntity(l, cancellationToken),
            () => Task.FromResult<Result<Like, LikeException>>(new LikeNotFoundException(likeId)));
    }

    public async Task<Result<Like, LikeException>> DeleteEntity(Like like, CancellationToken cancellationToken)
    {
        try
        {
            return await likeRepository.Delete(like, cancellationToken);
        }
        catch (Exception exception)
        {
            return new LikeUnknownException(like.Id, exception);
        }
    }
}