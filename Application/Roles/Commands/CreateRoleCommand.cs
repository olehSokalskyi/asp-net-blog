using Application.Common;
using Application.Common.Interfaces;
using Application.Roles.Exceptions;
using Domain.Roles;
using MediatR;

namespace Application.Roles;

public class CreateRoleCommand : IRequest<Result<Role, RoleException>>
{
    public required string Name { get; init; }
}

public class CreateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<CreateRoleCommand, Result<Role, RoleException>>
{
    public async Task<Result<Role, RoleException>> Handle(CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var existRole = await roleRepository.GetByName(request.Name, cancellationToken);

            return await existRole.Match(
                r => Task.FromResult<Result<Role, RoleException>>(
                    new RoleAlreadyExistsException(r.Name)),
                async () => await CreateEntity(request.Name, cancellationToken));
        }
        catch (Exception exception)
        {
            return new RoleUnknownException(RoleId.Empty(), exception);
        }
    }

    private async Task<Result<Role, RoleException>> CreateEntity(string name, CancellationToken cancellationToken)
    {
        try
        {
            var role = Role.New(new RoleId(Guid.NewGuid()), name);
            return await roleRepository.Add(role, cancellationToken);
        }
        catch (Exception exception)
        {
            return new RoleUnknownException(RoleId.Empty(), exception);
        }
    }
}