using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Application.Users.Models;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class LoginUserCommand : IRequest<Result<AuthResult, UserException>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    ITokenGenerator tokenGenerator,
    IPasswordHasher passwordHasher,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<LoginUserCommand, Result<AuthResult, UserException>>
{
    public async Task<Result<AuthResult, UserException>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetByEmail(request.Email, cancellationToken);
            return await user.Match(
                async u => await Authenticate(u, request.Password),
                () => Task.FromResult<Result<AuthResult, UserException>>(new UserNotFoundException(UserId.Empty())));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<AuthResult, UserException>> Authenticate(User user, string password)
    {
        try
        {
            if (passwordHasher.VerifyPassword(password, user.Password))
            {
                var token = tokenGenerator.GenerateToken(user);
                var refreshToken = await refreshTokenRepository.Add(refreshTokenGenerator.Generate(user.Id), CancellationToken.None);

                user.AddRefreshToken(refreshToken);
                await userRepository.Update(user, CancellationToken.None);

                var authResult = new AuthResult(token, refreshToken.Token);
                return authResult;
            }

            return new UserIncorrectPasswordException(user.Id);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}