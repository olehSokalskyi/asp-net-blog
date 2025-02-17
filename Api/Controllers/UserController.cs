﻿using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Users.Commands;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
[ApiController]
public class UserController(ISender sender, IUserQueries userQueries, IJwtDecoder jwtDecoder) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await userQueries.GetAll(cancellationToken);
        
        return entities.Select(UserDto.FromDomainModel).ToList();
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDto>> GetById(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var entity = await userQueries.GetById(new UserId(userId), cancellationToken);
        
        return entity.Match<ActionResult<UserDto>>(
            l => UserDto.FromDomainModel(l),
            () => NotFound());
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Add(
        [FromBody] CreateUserDto userDto,
        CancellationToken cancellationToken)
    {
        var input = new CreateUserCommand
        {
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Password = userDto.Password,
            Username = userDto.Username
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize]
    [HttpPut("update-email")]
    public async Task<ActionResult<UserDto>> UpdateEmail(
        [FromBody] UserUpdateEmailDto userDto,
        CancellationToken cancellationToken)
    {
        var userId = Request.GetUserIdFromToken(jwtDecoder);

        var input = new UpdateUserEmailCommand
        {
            Email = userDto.Email,
            UserId = userId
        };
        
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize]
    [HttpPut("update-data")]
    public async Task<ActionResult<UserDto>> UpdateData(
        [FromBody] UserDto userDto,
        CancellationToken cancellationToken)
    {
        var userId = Request.GetUserIdFromToken(jwtDecoder);

        var input = new UpdateUserDataCommand
        {
            UserId = userId,
            Username = userDto.Username,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            GenderId = userDto.GenderId.Value
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize]
    [HttpPut("update-password")]
    public async Task<ActionResult<UserDto>> UpdatePassword(
        [FromBody] UserUpdatePasswordDto userUpdatePasswordDto,
        CancellationToken cancellationToken)
    {
        var userId = Request.GetUserIdFromToken(jwtDecoder);

        var input = new UpdateUserPasswordCommand
        {
            UserId = userId,
            Password = userUpdatePasswordDto.CurrentPassword,
            NewPassword = userUpdatePasswordDto.NewPassword
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize]
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<UserDto>> Delete(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var input = new DeleteUserCommand
        {
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(
        [FromBody] UserLoginDto userLoginDto,
        CancellationToken cancellationToken)
    {
        var input = new LoginUserCommand
        {
            Email = userLoginDto.email,
            Password = userLoginDto.Password
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TokenDto>>(
            u => Ok(new TokenDto(u.Token, u.RefreshToken)),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update-role")]
    public async Task<ActionResult<UserDto>> UpdateRole(
        [FromBody] UserUpdateRoleDto userUpdateRoleDto,
        CancellationToken cancellationToken)
    {
        var input = new UpdateUserRoleCommand
        {
            RoleId = userUpdateRoleDto.RoleId,
            UserId = userUpdateRoleDto.UserId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken(
        [FromBody] RefreshTokenDto refreshTokenDto,
        CancellationToken cancellationToken)
    {
        var input = new RegenerateRefreshTokenCommand
        {
            Token = refreshTokenDto.RefreshToken,
            UserId = refreshTokenDto.UserId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TokenDto>>(
            u => Ok(new TokenDto(u.Token, u.RefreshToken)),
            e => e.ToObjectResult());
    }

    [HttpDelete("logout")]
    public async Task<ActionResult<Unit>> Logout(
        [FromBody] RefreshTokenDto refreshTokenDto,
        CancellationToken cancellationToken)
    {
        var input = new LogoutCommand
        {
            Token = refreshTokenDto.RefreshToken,
            UserId = refreshTokenDto.UserId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<Unit>>(
            u => Ok("Logged out"),
            e => e.ToObjectResult());
    }
}