﻿using Domain.Genders;
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
    RoleDto? Role,
    Guid? GenderId,
    GenderDto Gender
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
            Role: RoleDto.FromDomainModel(user.Role),
            GenderId: user.GenderId.Value,
            Gender: GenderDto.FromDomainModel(user.Gender)
            
            );
}
public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Password,
    Guid GenderId
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
public record TokenDto(
    string Token
);