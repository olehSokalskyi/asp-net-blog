using Domain.Genders;
using Domain.Roles;
using Domain.Users;

namespace Application.Users.Exceptions;

public abstract class UserException(UserId id, string message, Exception? innerException = null ) 
    : Exception(message,innerException)
{
    public UserId UserId { get; } = id;
}

public class UserNotFoundException(UserId id) : UserException(id, $"User under id: {id} not found");

public class UserAlreadyExistsException(UserId id) : UserException(id, $"User already exists: {id}");

public class UserWithUsernameAlreadyExistsException(string username) 
    : UserException(UserId.Empty(), $"User with username already exists: {username}");

public class UserWithEmailAlreadyExistsException(string email) 
    : UserException(UserId.Empty(), $"User with email already exists: {email}");

public class UserWithUsernameNotFoundException(string username) 
    : UserException(UserId.Empty(), $"User with username not found: {username}");

public class UserIncorrectPasswordException(UserId id) 
    : UserException(id, $"Incorrect password or email for the user under id: {id}");
public class UserUnknownException(UserId id, Exception innerException)
    : UserException(id, $"Unknown exception for the user under id: {id}", innerException);

public class UserPasswordSameAsCurrentException(UserId id)  
    : UserException(id, $"New password is the same as the current password for the user under id: {id}");
public class UserRoleNotFoundException(string roleName) 
    : UserException(UserId.Empty(), $"Role with name: {roleName} not found");
    
public class UserRoleNotFoundExceptionById(RoleId roleId):
    UserException(UserId.Empty(), $"Role with id: {roleId} not found");

public class GenderNotFoundException(GenderId genderId):
    UserException(UserId.Empty(), $"Gender with id: {genderId} not found");

public class UserRefreshTokenNotFoundException(UserId userId, string token) 
    : UserException(userId, $"Refresh token with token: {token} not found for user under id: {userId}");

public class UserRefreshTokenNotActiveException (UserId userId, string token)
    : UserException(userId, $"Refresh token with token: {token} is not active for user under id: {userId}");
    