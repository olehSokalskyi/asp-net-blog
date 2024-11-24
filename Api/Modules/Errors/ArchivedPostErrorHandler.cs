using Application.ArchivedPosts.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ArchivedPostErrorHandler
{
    public static ObjectResult ToObjectResult(this ArchivedPostException exception)
    {
        return new ObjectResult(exception.ArchivedPostId)
        {
            StatusCode = exception switch
            {
                ArchivedPostNotFoundException => StatusCodes.Status404NotFound,
                ArchivedPostUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Archived post error handler does not implemented")
            }
        };
    }
}