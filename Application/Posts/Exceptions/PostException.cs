using Domain.Posts;
using Domain.Users;

namespace Application.Posts.Exceptions;

public abstract class PostException(PostId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public PostId CategoryId { get; } = id;
}

public class PostNotFoundException(PostId id) : PostException(id, $"Post under id: {id} not found");

public class PostUserNotFoundException(PostId postId, UserId userId)
    : PostException(postId, $"User under id: {userId} not found");

public class PostFailedToUploadImage(PostId id) : PostException(id, $"Image upload failed to upload");

public class PostUnknownException(PostId id, Exception innerException)
    : PostException(id, $"Unknown exception for the post under id: {id}", innerException);