using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserDataCommand: IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    
}

public class UpdateUserDataCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserDataCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UpdateUserDataCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
            return await user.Match(
                u => UpdateEntity(u, request.Username, request.FirstName, request.LastName, cancellationToken),
                () => Task.FromResult<Result<User, UserException>>(new UserNotFoundException(new UserId(request.UserId))));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<User, UserException>> UpdateEntity(User user, string username, string firstName, string lastName,
        CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateDetails(username, firstName, lastName);
            return await userRepository.Update(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}