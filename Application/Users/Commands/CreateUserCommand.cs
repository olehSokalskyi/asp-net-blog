using Application.Common;
using Application.Common.Interfaces;
using Application.Users.Exceptions;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand : IRequest<Result<User, UserException>>
{
    public required string Username { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
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
                async () => await CreateEntity(request.Username, request.FirstName, request.LastName, request.Email,
                    request.Password, cancellationToken));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<User, UserException>> CreateEntity(
        string username,
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        try
        {
            var hashedPassword = passwordHasher.HashPassword(password);
            var entity = User.New(UserId.New(), username, firstName, lastName, email, hashedPassword);

            return await userRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }
}