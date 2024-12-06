using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Roles;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserRoleCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required Guid RoleId { get; init; }
}

public class UpdateUserRoleCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : IRequestHandler<UpdateUserRoleCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UpdateUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
        return await user.Match(
            async u =>
            {
                var role = await roleRepository.GetById(new RoleId(request.RoleId), cancellationToken);

                return await role.Match(
                    async r => await UpdateEntity(u, r, cancellationToken),
                    () => Task.FromResult<Result<User, UserException>>(
                        new UserRoleNotFoundExceptionById(new RoleId(request.RoleId))));
            },
            () => Task.FromResult<Result<User, UserException>>(
                new UserNotFoundException(new UserId(request.UserId))));
    }

    private async Task<Result<User, UserException>> UpdateEntity(User user, Role role,
        CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateRole(role.Id);
            
            return await userRepository.Update(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}