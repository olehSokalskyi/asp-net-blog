using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Roles;
using Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("roles")]
[ApiController]
public class RoleController(ISender sender, IRoleQueries roleQueries) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await roleQueries.GetAll(cancellationToken);
        return entities.Select(RoleDto.FromDomainModel).ToList();
    }
    
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Add([FromBody] RoleDto request, CancellationToken cancellationToken)
    {
        var input = new CreateRoleCommand
        {
            Name = request.Name
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    public async Task<ActionResult<RoleDto>> UpdateName([FromBody] RoleDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateRoleCommand
        {
            RoleId = request.Id!.Value,
            Name = request.Name
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> Delete([FromRoute] Guid roleId, CancellationToken cancellationToken)
    {
        var input = new DeleteRoleCommand
        {
            RoleId = roleId
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
    
 
    
}