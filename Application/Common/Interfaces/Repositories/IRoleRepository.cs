using Domain.Roles;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Option<Role>> GetById(RoleId id, CancellationToken cancellationToken);
    Task<Option<Role>> GetByName(string name, CancellationToken cancellationToken);
    
    Task<Role> Add(Role role, CancellationToken cancellationToken);
    Task<Role> Update(Role role, CancellationToken cancellationToken);
    Task<Role> Delete(Role role, CancellationToken cancellationToken);
}