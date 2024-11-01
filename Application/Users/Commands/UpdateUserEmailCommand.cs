using Application.Common;
using Application.Common.Interfaces;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserEmailCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required string Email { get; init; }
}

public class UpdateUserEmailCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserEmailCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UpdateUserEmailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
            return await user.Match(
                async u =>
                {
                    var userExistEmail = await userRepository.GetByEmail(request.Email, cancellationToken);
                    return await userExistEmail.Match(
                        existUser =>
                            Task.FromResult<Result<User, UserException>>(
                                new UserWithEmailAlreadyExistsException(existUser.Email)),
                        () => UpdateEntity(u, request.Email, cancellationToken));
                },
                () => Task.FromResult<Result<User, UserException>>(
                    new UserNotFoundException(new UserId(request.UserId))));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<User, UserException>> UpdateEntity(User user, string email,
        CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateEmail(email);
            return await userRepository.Update(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}