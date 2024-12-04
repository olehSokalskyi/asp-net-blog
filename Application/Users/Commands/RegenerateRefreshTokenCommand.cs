using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Application.Users.Models;
using Domain.RefreshTokens;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public record RegenerateRefreshTokenCommand : IRequest<Result<AuthResult, UserException>>
{
    public Guid UserId { get; init; }
    public string Token { get; init; }
}

public class RegenerateRefreshTokenCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IRefreshTokenGenerator refreshTokenGenerator,
    ITokenGenerator tokenGenerator)
    : IRequestHandler<RegenerateRefreshTokenCommand, Result<AuthResult, UserException>>
{
    public async Task<Result<AuthResult, UserException>> Handle(
        RegenerateRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);

        return await user.Match(
            async u =>
            {
                var refreshToken = await refreshTokenRepository.GetByToken(request.Token, CancellationToken.None);
                return await refreshToken.Match(
                    rf => RegenerateToken(u, rf),
                    () => Task.FromResult<Result<AuthResult, UserException>>(
                        new UserRefreshTokenNotFoundException(u.Id, request.Token)));
            },
            () => Task.FromResult<Result<AuthResult, UserException>>(
                new UserNotFoundException(new UserId(request.UserId))));
    }

    private async Task<Result<AuthResult, UserException>> RegenerateToken(
        User user,
        RefreshToken token)
    {
        try
        {
            if (token.UserId != user.Id)
                return new UserNotFoundException(user.Id);
            if (!token.IsActive)
                return new UserRefreshTokenNotActiveException(user.Id, token.Token);
            var jwtToken = tokenGenerator.GenerateToken(user);
            var newToken = refreshTokenGenerator.Update(token);
            await refreshTokenRepository.Update(newToken, CancellationToken.None);

            return new AuthResult(jwtToken, newToken.Token);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}