using Application.Posts.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PostErrorHandler
{
    public static ObjectResult ToObjectResult(this PostException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                PostNotFoundException
                    or PostUserNotFoundException => StatusCodes.Status404NotFound,
                PostUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Post error handler does not implemented")
            }
        };
    }
}