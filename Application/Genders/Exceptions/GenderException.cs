using Domain.Genders;

namespace Application.Genders.Exceptions;

public abstract class GenderException(GenderId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public GenderId GenderId { get; } = id;
}

public class GenderNotFoundException(GenderId id) : GenderException(id, $"Gender under id: {id} not found");

public class GenderAlreadyExistsException(GenderId id) : GenderException(id, $"Gender already exists: {id}");

public class GenderUnknownException(GenderId id, Exception innerException)
    : GenderException(id, $"Unknown exception for the gender under id: {id}", innerException);