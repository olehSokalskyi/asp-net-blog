using Domain.Roles;

namespace Application.Roles.Exceptions;

public abstract class RoleException(RoleId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public RoleId RoleId { get; } = id;
}

public class RoleNotFoundException(RoleId id) : RoleException(id, $"Role under id: {id} not found");

public class RoleAlreadyExistsException(string name)
    : RoleException(RoleId.Empty(), $"Role with name already exists: {name}");

public class RoleWithNameNotFoundException(string name)
    : RoleException(RoleId.Empty(), $"Role with name not found: {name}");

public class RoleUnknownException(RoleId id, Exception innerException)
    : RoleException(id, $"Unknown exception for the role under id: {id}", innerException);