using Application.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class UserErrorHandler
{
    public static ObjectResult ToObjectResult(this UserException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                UserAlreadyExistsException or UserWithEmailAlreadyExistsException
                    or UserWithUsernameAlreadyExistsException => StatusCodes.Status409Conflict,
                UserNotFoundException or UserWithUsernameNotFoundException => StatusCodes.Status404NotFound,
                UserUnknownException => StatusCodes.Status500InternalServerError,
                UserIncorrectPasswordException => StatusCodes.Status401Unauthorized,
                _ => throw new NotImplementedException("User error handler does not implemented")
            }
        };
    }
}