﻿using System.Security.Claims;
using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Users.Commands;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

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
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<UserDto>> Add([FromBody] CreateUserDto userDto, CancellationToken cancellationToken)
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
    public async Task<ActionResult<UserDto>> UpdateEmail([FromBody] UserUpdateEmailDto userDto,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new UpdateUserEmailCommand
        {
            Email = userDto.Email,
            UserId = Guid.Parse(userIdClaim)
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize]
    [HttpPut("update-data")]
    public async Task<ActionResult<UserDto>> UpdateData([FromBody] UserDto userDto, CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new UpdateUserDataCommand
        {
            UserId = Guid.Parse(userIdClaim),
            Username = userDto.Username,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize]
    [HttpPut("update-password")]
    public async Task<ActionResult<UserDto>> UpdatePassword([FromBody] UserUpdatePasswordDto userUpdatePasswordDto,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var userIdClaim = jwtDecoder.DecodeToken(token).FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)
            ?.Value;


        var input = new UpdateUserPasswordCommand
        {
            UserId = Guid.Parse(userIdClaim),
            Password = userUpdatePasswordDto.CurrentPassword,
            NewPassword = userUpdatePasswordDto.NewPassword
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDto>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteUserCommand
        {
            UserId = id
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] UserLoginDto userLoginDto,
        CancellationToken cancellationToken)
    {
        var input = new LoginUserCommand
        {
            email = userLoginDto.email,
            password = userLoginDto.Password
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<TokenDto>>(
            u => Ok(new TokenDto(u)),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update-role")]
    public async Task<ActionResult<UserDto>> UpdateRole([FromBody] UserUpdateRoleDto userUpdateRoleDto,
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
}