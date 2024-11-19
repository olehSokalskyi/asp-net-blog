using Application.Subscribers.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class SubscriberErrorHandler
{
    public static ObjectResult ToObjectResult(this SubscriberException exception)
    {
        return new ObjectResult(exception.SubscriberId)
        {
            StatusCode = exception switch
            {
                SubscriberNotFoundException => StatusCodes.Status404NotFound,
                SubscriberUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Subscriber error handler does not implemented")
            }
        };
    }
}