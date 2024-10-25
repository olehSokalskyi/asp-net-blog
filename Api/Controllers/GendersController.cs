using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Genders.Commands;
using Domain.Genders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("genders")]
[ApiController]
public class GendersController(ISender sender, IGenderQueries genderQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GenderDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var entities = await genderQueries.GetAll(cancellationToken);

        return entities.Select(GenderDto.FromDomainModel).ToList();
    }
    
    [HttpGet("{genderId:guid}")]
    public async Task<ActionResult<GenderDto>> GetById(
        [FromRoute] Guid genderId, 
        CancellationToken cancellationToken)
    {
        var entity = await genderQueries.GetById(new GenderId(genderId), cancellationToken);

        return entity.Match<ActionResult<GenderDto>>(
            g => GenderDto.FromDomainModel(g),
            () => NotFound());
    }
    
    [HttpGet]
    public async Task<ActionResult<GenderDto>> GetByName(
        [FromRoute] string title, 
        CancellationToken cancellationToken)
    {
        var entity = await genderQueries.GetByName(new string(title), cancellationToken);

        return entity.Match<ActionResult<GenderDto>>(
            g => GenderDto.FromDomainModel(g),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<GenderDto>> Create(
        [FromBody] GenderDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateGenderCommand
        {
            Title = request.Title
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<GenderDto>>(
            g => GenderDto.FromDomainModel(g),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<GenderDto>> Update(
        [FromBody] GenderDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateGenderCommand
        {
            GenderId = request.Id!.Value,
            Title = request.Title
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<GenderDto>>(
            g => GenderDto.FromDomainModel(g),
            e => e.ToObjectResult());
    }
    
    [HttpDelete]
    public async Task<ActionResult<GenderDto>> Delete(
        [FromRoute] Guid genderId, 
        CancellationToken cancellationToken)
    {
        var input = new DeleteGenderCommand()
        {
            GenderId = genderId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<GenderDto>>(
            g => GenderDto.FromDomainModel(g),
            e => e.ToObjectResult());
    }
}   