using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.ArchivedPosts.Commands;
using Domain.ArchivedPosts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("archivedPosts")]
[ApiController]
public class ArchivedPostsController(ISender sender, IArchivedPostQueries archivedPostQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ArchivedPostDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await archivedPostQueries.GetAll(cancellationToken);

        return entities.Select(ArchivedPostDto.FromDomainModel).ToList();
    }
    
    [HttpGet("{archivedPostId:guid}")]
    public async Task<ActionResult<ArchivedPostDto>> GetById(
        [FromRoute] Guid archivedPostId, 
        CancellationToken cancellationToken)
    {
        var entity = await archivedPostQueries.GetById(new ArchivedPostId(archivedPostId), cancellationToken);

        return entity.Match<ActionResult<ArchivedPostDto>>(
            a => ArchivedPostDto.FromDomainModel(a),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<ArchivedPostDto>> Create(
        [FromBody] ArchivedPostDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateArchivedPostCommand
        {
            PostId = request.PostId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ArchivedPostDto>>(
            a => ArchivedPostDto.FromDomainModel(a),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{archivedPostId:guid}")]
    public async Task<ActionResult<ArchivedPostDto>> Delete(
        [FromRoute] Guid archivedPostId, 
        CancellationToken cancellationToken)
    {
        var input = new DeleteArchivedPostCommand
        {
            ArchivedPostsId = archivedPostId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ArchivedPostDto>>(
            a => ArchivedPostDto.FromDomainModel(a),
            e => e.ToObjectResult());
    }
}   