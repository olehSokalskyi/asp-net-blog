using Application.Common;
using Application.Common.Interfaces;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles;

public class UpdateRoleCommand: IRequest<Result<Role,RoleException>>
{
    public required Guid RoleId { get; init; }
    public required string Name { get; init; }
}

public class UpdateRoleCommandHandler(IRoleRepository roleRepository) :
    IRequestHandler<UpdateRoleCommand, Result<Role, RoleException>>
{
    public async  Task<Result<Role, RoleException>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var existRole = await roleRepository.GetById(new RoleId(request.RoleId), cancellationToken);
        
        return await existRole.Match(
            async r =>
            {
                var existRoleName = await roleRepository.GetByName(request.Name, cancellationToken);
                return await existRoleName.Match(
                    role => Task.FromResult<Result<Role, RoleException>>(new RoleAlreadyExistsException(role.Name)),
                    async () => await UpdateEntity(r, request.Name, cancellationToken));

            },
            () => Task.FromResult<Result<Role, RoleException>>(new RoleNotFoundException(new RoleId(request.RoleId))));
    }
    
    private async Task<Result<Role,RoleException>> UpdateEntity(Role role, string name, CancellationToken cancellationToken)
    {
        try
        {
            role.UpdateName(name);
            return await roleRepository.Update(role, cancellationToken);
        }
        catch (Exception exception)
        {
            return new RoleUnknownException(role.Id, exception);
        }
    }
}