using Application.Roles.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class RoleErrorHandler
{
    public static ObjectResult ToObjectResult(this RoleException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                RoleAlreadyExistsException => StatusCodes.Status409Conflict,
                RoleNotFoundException => StatusCodes.Status404NotFound,
                RoleUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Role error handler does not implemented")
            }
        };
    }
}