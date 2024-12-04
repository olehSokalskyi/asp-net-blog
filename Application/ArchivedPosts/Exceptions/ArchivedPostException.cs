using Domain.ArchivedPosts;
using Domain.Posts;

namespace Application.ArchivedPosts.Exceptions;

public abstract class ArchivedPostException(ArchivedPostId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ArchivedPostId ArchivedPostId { get; } = id;
}

public class ArchivedPostNotFoundException(ArchivedPostId id)
    : ArchivedPostException(id, $"Archived post under id: {id} not found");

public class ArchivedPostAlreadyExistsException(ArchivedPostId id)
    : ArchivedPostException(id, $"Archived post already exists: {id}");

public class ArchivedPostUnknownException(ArchivedPostId id, Exception innerException)
    : ArchivedPostException(id, $"Unknown exception for the archived post under id: {id}", innerException);

public class ArchivedPostForPostNotFoundException(PostId postId)
    : ArchivedPostException(ArchivedPostId.Empty(), $"Post under id:{postId} not found");