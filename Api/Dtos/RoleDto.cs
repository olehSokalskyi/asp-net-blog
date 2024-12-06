using Domain.Roles;

namespace Api.Dtos;

public record RoleDto(Guid? Id, string? Name)
{
    public static RoleDto FromDomainModel(Role? role)
        => new(
            Id: role.Id?.Value,
            Name: role?.Name
        );
}