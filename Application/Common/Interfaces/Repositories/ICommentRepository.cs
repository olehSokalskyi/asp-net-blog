using Domain.Comments;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<Option<Comment>> GetByCommentAndUserId(CommentId commentId, UserId userId, CancellationToken cancellationToken);
    
    Task<Comment> Add(Comment comment, CancellationToken cancellationToken);
    Task<Comment> Update(Comment comment, CancellationToken cancellationToken);
    Task<Comment> Delete(Comment comment, CancellationToken cancellationToken);
}