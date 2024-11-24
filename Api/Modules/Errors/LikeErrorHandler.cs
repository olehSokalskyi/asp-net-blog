using Application.Likes.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class LikeErrorHandler
{
    public static ObjectResult ToObjectResult(this LikeException exception)
    {
        return new ObjectResult(exception.LikeId)
        {
            StatusCode = exception switch
            {
                LikeNotFoundException => StatusCodes.Status404NotFound,
                LikeUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Like error handler does not implemented")
            }
        };
    }
}