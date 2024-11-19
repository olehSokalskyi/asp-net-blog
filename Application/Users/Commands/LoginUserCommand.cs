using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class LoginUserCommand : IRequest<Result<string, UserException>>
{
    public required string email { get; init; }
    public required string password { get; init; }
}

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    ITokenGenerator tokenGenerator,
    IPasswordHasher passwordHasher)
    : IRequestHandler<LoginUserCommand, Result<string, UserException>>
{
    public async Task<Result<string, UserException>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetByEmail(request.email, cancellationToken);
            return await user.Match(
                async u => await Authenticate(u, request.password),
                () => Task.FromResult<Result<string, UserException>>(new UserNotFoundException(UserId.Empty())));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<string, UserException>> Authenticate(User user, string password)
    {
        try
        {
            if (passwordHasher.VerifyPassword(password, user.Password))
            {
                var token = tokenGenerator.GenerateToken(user);
                return token;
            }

            return new UserIncorrectPasswordException(user.Id);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}