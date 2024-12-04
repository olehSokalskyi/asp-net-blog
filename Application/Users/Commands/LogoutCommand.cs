using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public record LogoutCommand : IRequest<Result<Unit, UserException>>
{
    public Guid UserId { get; init; }
    public string Token { get; init; }
}

public class LogoutCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<LogoutCommand, Result<Unit, UserException>>
{
    public async Task<Result<Unit, UserException>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
        return await user.Match(
            async u =>
            {
                var refreshToken = await refreshTokenRepository.GetByToken(request.Token, CancellationToken.None);
                return await refreshToken.Match(
                    rf => RevokeToken(u, rf),
                    () => Task.FromResult<Result<Unit, UserException>>(new UserNotFoundException(u.Id)));
            },
            () => Task.FromResult<Result<Unit, UserException>>(new UserNotFoundException(new UserId(request.UserId))));
    }
    
    private async Task<Result<Unit, UserException>> RevokeToken(User user, RefreshToken token)
    {
        try
        {
            if(token.UserId != user.Id)
                return new UserNotFoundException(user.Id);
           
            await refreshTokenRepository.Delete(token, CancellationToken.None);
        
            return Unit.Value;
            
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
       
    }
}