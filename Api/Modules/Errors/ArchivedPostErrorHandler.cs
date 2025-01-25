using Application.ArchivedPosts.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ArchivedPostErrorHandler
{
    public static ObjectResult ToObjectResult(this ArchivedPostException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                ArchivedPostNotFoundException
                    or ArchivedPostForPostNotFoundException => StatusCodes.Status404NotFound,
                ArchivedPostAlreadyExistsException => StatusCodes.Status409Conflict,
                ArchivedPostUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Archived post error handler does not implemented")
            }
        };
    }
}