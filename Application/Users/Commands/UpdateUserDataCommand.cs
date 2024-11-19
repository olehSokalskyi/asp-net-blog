using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Genders;
using Domain.Roles;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public class UpdateUserDataCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Guid GenderId { get; init; }
}

public class UpdateUserDataCommandHandler(IUserRepository userRepository, IGenderRepository genderRepository)
    : IRequestHandler<UpdateUserDataCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UpdateUserDataCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var gender = await genderRepository.GetById(new GenderId(request.GenderId), cancellationToken);
            return await gender.Match(
                async g =>
                {
                    var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
                    return await user.Match(
                        u => UpdateEntity(u, request.Username, request.FirstName, request.LastName,g, cancellationToken),
                        () => Task.FromResult<Result<User, UserException>>(
                            new UserNotFoundException(new UserId(request.UserId))));
                },
                () => Task.FromResult<Result<User, UserException>>(
                    new GenderNotFoundException(new GenderId(request.GenderId))));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty(), exception);
        }
    }

    private async Task<Result<User, UserException>> UpdateEntity(User user, string username, string firstName,
        string lastName, Gender gender,
            CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateDetails(username, firstName, lastName, gender.Id);
            return await userRepository.Update(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}