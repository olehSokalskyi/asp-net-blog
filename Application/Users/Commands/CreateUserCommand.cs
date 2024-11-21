using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Genders;
using Domain.Roles;
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

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IGenderRepository genderRepository)
    : IRequestHandler<CreateUserCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var role = await roleRepository.GetByName("User", cancellationToken);

            return await role.Match(
                async r =>
                {
                    
                var existingUserEmail = await userRepository.GetByEmail(request.Email, cancellationToken);

                return await existingUserEmail.Match(
                    u => Task.FromResult<Result<User, UserException>>(
                        new UserWithEmailAlreadyExistsException(u.Email)),
                    async () =>
                    {
                        var existingUserUserName =
                            await userRepository.GetByUsername(request.Username, cancellationToken);
                        return await existingUserUserName.Match(
                            u => Task.FromResult<Result<User, UserException>>(
                                new UserWithUsernameAlreadyExistsException(u.Username)),
                            async () => await CreateEntity(request.Username, request.FirstName, request.LastName,
                                request.Email, request.Password, r, cancellationToken));
                    });
                },
                () => Task.FromResult<Result<User, UserException>>(new UserRoleNotFoundException("User")));
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
        Role role,
        CancellationToken cancellationToken)
    {
        try
        {
            var hashedPassword = passwordHasher.HashPassword(password);


            var entity = User.New(UserId.New(), username, firstName, lastName, email, hashedPassword, "", role.Id);

            return await userRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }
}