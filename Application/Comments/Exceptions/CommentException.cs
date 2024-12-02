using Domain.Comments;
using Domain.Posts;
using Domain.Users;

namespace Application.Comments.Exceptions;

public abstract class CommentException(CommentId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public CommentId CommentId { get; } = id;
}

public class CommentNotFoundException(CommentId id) : CommentException(id, $"Comment under id: {id} not found");

public class CommentUserNotFoundException(CommentId commentId, UserId userId)
    : CommentException(commentId, $"User under id: {userId} not found");

public class CommentPostNotFoundException(CommentId commentId, PostId postId)
    : CommentException(commentId, $"Post under id: {postId} not found");

public class CommentUnknownException(CommentId id, Exception innerException)
    : CommentException(id, $"Unknown exception for the comment under id: {id}", innerException);