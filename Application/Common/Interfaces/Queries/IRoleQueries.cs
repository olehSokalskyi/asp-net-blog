using Domain.Roles;

namespace Application.Common.Interfaces.Queries;

public interface IRoleQueries
{
    Task<IReadOnlyList<Role>> GetAll(CancellationToken cancellationToken);
}