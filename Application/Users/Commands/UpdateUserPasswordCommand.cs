using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserPasswordCommand: IRequest<Result<User,UserException>>
{
    public required Guid UserId { get; set; }
    public required string Password { get; set; }
    public required string NewPassword { get; set; }
}

public class UpdateUserPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<UpdateUserPasswordCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
            return await user.Match(
                u => UpdateEntity(u, request.Password, request.NewPassword, cancellationToken),
                () => Task.FromResult<Result<User, UserException>>(new UserNotFoundException(new UserId(request.UserId))));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<User, UserException>> UpdateEntity(User user, string password, string newPassword,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!passwordHasher.VerifyPassword(password, user.Password))
            {
                return new UserIncorrectPasswordException(user.Id);
            }
            if(passwordHasher.VerifyPassword(newPassword, user.Password))
            {
                return new UserPasswordSameAsCurrentException(user.Id);
            }
            var hashedPassword = passwordHasher.HashPassword(newPassword);
            user.UpdatePassword(hashedPassword);
            return await userRepository.Update(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}
