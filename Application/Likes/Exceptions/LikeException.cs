using Application.Messages.Exceptions;
using Domain.Likes;
using Domain.Users;

namespace Application.Likes.Exceptions;

public abstract class LikeException(LikeId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public LikeId LikeId { get; } = id;
}

public class LikeNotFoundException(LikeId id) : LikeException(id, $"Like under id: {id} not found");

public class LikeAlreadyExistsException(LikeId id) : LikeException(id, $"Like already exists: {id}");

public class LikeUnknownException(LikeId id, Exception innerException)
    : LikeException(id, $"Unknown exception for the like under id: {id}", innerException);
    
public class LikeUserNotFoundException(UserId userId)
    : LikeException(LikeId.Empty(), $"User under id:{userId} not found");

/*public class LikePostNotFoundException(PostId postId)
    : LikeException(LikeId.Empty(), $"Post under id:{postId} not found");*/