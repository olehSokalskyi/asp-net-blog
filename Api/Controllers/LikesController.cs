using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Genders.Commands;
using Application.Likes.Commands;
using Domain.Genders;
using Domain.Likes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("likes")]
[ApiController]
public class LikesController(ISender sender, ILikeQueries likeQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LikeDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var entities = await likeQueries.GetAll(cancellationToken);

        return entities.Select(LikeDto.FromDomainModel).ToList();
    }
    
    [HttpGet("{likeId:guid}")]
    public async Task<ActionResult<LikeDto>> GetById(
        [FromRoute] Guid likeId, 
        CancellationToken cancellationToken)
    {
        var entity = await likeQueries.GetById(new LikeId(likeId), cancellationToken);

        return entity.Match<ActionResult<LikeDto>>(
            l => LikeDto.FromDomainModel(l),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<LikeDto>> Create(
        [FromBody] LikeDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateLikeCommand
        {
            UserId = request.UserId,
            PostId = request.PostId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<LikeDto>>(
            l => LikeDto.FromDomainModel(l),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{likeId:guid}")]
    public async Task<ActionResult<LikeDto>> Delete(
        [FromRoute] Guid likeId, 
        CancellationToken cancellationToken)
    {
        var input = new DeleteLikeCommand()
        {
            LikeId = likeId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<LikeDto>>(
            l => LikeDto.FromDomainModel(l),
            e => e.ToObjectResult());
    }
}   