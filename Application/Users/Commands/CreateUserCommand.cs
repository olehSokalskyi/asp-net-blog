using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand : IRequest<Result<User, UserException>>
{
    public required string Username { get; init; }
}

public class CreateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await userRepository.GetByUsername(request.Username, cancellationToken);

            return await existingUser.Match(
                u => Task.FromResult<Result<User, UserException>>(new UserAlreadyExistsException(u.Id)),
                async () => await CreateEntity(request.Username, cancellationToken));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<User, UserException>> CreateEntity(
        string username,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = User.New(UserId.New(), username);

            return await userRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }
}