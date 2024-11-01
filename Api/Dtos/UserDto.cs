using Domain.Roles;
using Domain.Users;

namespace Api.Dtos;

public record UserDto(
    Guid? Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? Username,
    string? ProfilePicture,
    DateTime? UpdatedAt,
    DateTime? CreatedAt,
    Guid? RoleId,
    Role? Role
)
{
    public static UserDto FromDomainModel(User user)
        => new(
            Id: user.Id.Value,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email,
            Username: user.Username,
            ProfilePicture: user.ProfilePicture,
            UpdatedAt: user.UpdatedAt,
            CreatedAt: user.CreatedAt,
            RoleId: user.RoleId.Value,
            Role: user.Role
            );
}
public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Password
);
public record UserUpdatePasswordDto(
    string CurrentPassword,
    string NewPassword
);

public record UserUpdateEmailDto(
    string Email
);

public record UserLoginDto(
    string email,
    string Password
);

public record UserUpdateRoleDto(
    Guid RoleId,
    Guid UserId
);