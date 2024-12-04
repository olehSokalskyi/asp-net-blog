using Application.Comments.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CommentErrorHandler
{
    public static ObjectResult ToObjectResult(this CommentException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                CommentNotFoundException
                    or CommentUserNotFoundException
                    or CommentPostNotFoundException => StatusCodes.Status404NotFound,
                CommentUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Comment error handler does not implemented")
            }
        };
    }
}