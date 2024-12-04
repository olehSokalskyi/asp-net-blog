using Api.Dtos;
using Api.Modules.Errors;
using Application.Comments.Commands;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[Authorize]
[Route("comments")]
[ApiController]
public class CommentsController(
    ISender sender,
    ICommentQueries commentQueries,
    IJwtDecoder jwtDecoder) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CommentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await commentQueries.GetAll(cancellationToken);
        return entities.Select(CommentDto.FromDomainModel).ToList();
    }

    [HttpGet("{commentId:guid}")]
    public async Task<ActionResult<CommentDto>> GetById(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var entity = await commentQueries.GetById(new CommentId(commentId), cancellationToken);

        return entity.Match<ActionResult<CommentDto>>(
            u => CommentDto.FromDomainModel(u),
            () => NotFound());
    }
    
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(
        [FromBody] CommentDto request,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new CreateCommentCommand
        {
            Body = request.Body!,
            UserId = Guid.Parse(userIdClaim),
            PostId = request.PostId!.Value,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CommentDto>>(
            f => CommentDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    public async Task<ActionResult<CommentDto>> Update(
        [FromBody] CommentDto request,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new UpdateCommentCommand
        {
            CommentId = request.Id!.Value,
            Body = request.Body!,
            UserId = Guid.Parse(userIdClaim)
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CommentDto>>(
            u => CommentDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [HttpDelete("{commentId:guid}")]
    public async Task<ActionResult<CommentDto>> Delete(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new DeleteCommentCommand
        {
            CommentId = commentId,
            UserId = Guid.Parse(userIdClaim)
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CommentDto>>(
            u => CommentDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }
}