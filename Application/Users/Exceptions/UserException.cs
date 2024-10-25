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
    