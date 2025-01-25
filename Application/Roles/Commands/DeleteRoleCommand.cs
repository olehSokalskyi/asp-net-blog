using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles.Commands;

public class DeleteRoleCommand : IRequest<Result<Role, RoleException>>
{
    public required Guid RoleId { get; init; }
}

public class DeleteRoleCommandHandler(IRoleRepository roleRepository) :
    IRequestHandler<DeleteRoleCommand, Result<Role, RoleException>>
{
    public async Task<Result<Role, RoleException>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var existRole = await roleRepository.GetById(new RoleId(request.RoleId), cancellationToken);

        return await existRole.Match(
            async r => await DeleteEntity(r, cancellationToken),
            () => Task.FromResult<Result<Role, RoleException>>(new RoleNotFoundException(new RoleId(request.RoleId))));
    }

    private async Task<Result<Role, RoleException>> DeleteEntity(
        Role role,
        CancellationToken cancellationToken)
    {
        try
        {
            return await roleRepository.Delete(role, cancellationToken);
        }
        catch (Exception exception)
        {
            return new RoleUnknownException(role.Id, exception);
        }
    }
}