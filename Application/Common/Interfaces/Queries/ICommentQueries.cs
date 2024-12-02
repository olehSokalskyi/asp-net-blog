using Domain.Comments;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICommentQueries
{
    Task<IReadOnlyList<Comment>> GetAll(CancellationToken cancellationToken);
    Task<Option<Comment>> GetById(CommentId id, CancellationToken cancellationToken);
}