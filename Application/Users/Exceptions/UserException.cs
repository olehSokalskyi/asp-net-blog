using Domain.Users;

namespace Application.Users.Exceptions;

public abstract class UserException(UserId id, string message, Exception? innerException = null ) 
    : Exception(message,innerException)
{
    public UserId UserId { get; } = id;
}

public class UserNotFoundException(UserId id) : UserException(id, $"User under id: {id} not found");

public class UserAlreadyExistsException(UserId id) : UserException(id, $"User already exists: {id}");
public class UserUnknownException(UserId id, Exception innerException)
    : UserException(id, $"Unknown exception for the user under id: {id}", innerException);
    