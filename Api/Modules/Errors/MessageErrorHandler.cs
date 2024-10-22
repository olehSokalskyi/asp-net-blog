using Application.Messages.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class MessageErrorHandler
{
    public static ObjectResult ToObjectResult(this MessageException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                MessageNotFoundException => StatusCodes.Status404NotFound,
                MessageUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Message error handler does not implemented")
            }
        };
    }
}