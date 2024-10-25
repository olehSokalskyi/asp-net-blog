using Application.Genders.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class GenderErrorHandler
{
    public static ObjectResult ToObjectResult(this GenderException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                GenderNotFoundException => StatusCodes.Status404NotFound,
                GenderAlreadyExistsException => StatusCodes.Status409Conflict,
                GenderUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Gender error handler does not implemented")
            }
        };
    }
}