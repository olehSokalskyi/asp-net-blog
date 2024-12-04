using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Likes.Commands;
using Domain.Likes;
using Domain.Posts;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[Authorize]
[Route("likes")]
[ApiController]
public class LikesController(
    ISender sender,
    ILikeQueries likeQueries,
    IJwtDecoder jwtDecoder) : ControllerBase
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

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IReadOnlyList<LikeDto>>> GetByUserId(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var entities = await likeQueries.GetByUserId(new UserId(userId), cancellationToken);
        return entities.Select(LikeDto.FromDomainModel).ToList();
    }

    [HttpGet("post/{postId:guid}")]
    public async Task<ActionResult<IReadOnlyList<LikeDto>>> GetByPostId(
        [FromRoute] Guid postId,
        CancellationToken cancellationToken)
    {
        var entities = await likeQueries.GetByPostId(new PostId(postId), cancellationToken);
        return entities.Select(LikeDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<LikeDto>> Create(
        [FromBody] LikeDto request,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        var input = new CreateLikeCommand
        {
            UserId = Guid.Parse(userIdClaim),
            PostId = request.PostId!.Value
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