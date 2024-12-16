using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Posts.Commands;
using Domain.Posts;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[Authorize]
[Route("posts")]
[ApiController]
public class PostsController(
    ISender sender,
    IPostQueries postQueries,
    IJwtDecoder jwtDecoder) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await postQueries.GetAll(cancellationToken);
        return entities.Select(PostDto.FromDomainModel).ToList();
    }

    [HttpGet("{postId:guid}")]
    public async Task<ActionResult<PostDto>> GetById(
        [FromRoute] Guid postId,
        CancellationToken cancellationToken)
    {
        var entity = await postQueries.GetById(new PostId(postId), cancellationToken);

        return entity.Match<ActionResult<PostDto>>(
            u => PostDto.FromDomainModel(u),
            () => NotFound());
    }
    
    [HttpGet("user/{userId:guid}")]
    public async Task<List<PostDto>> GetByUserId(
        [FromRoute] Guid userId, 
        CancellationToken cancellationToken)
    {
        var entities = await postQueries.GetByUserId(new UserId(userId), cancellationToken);
        return entities.Select(PostDto.FromDomainModel).ToList();
    }
    
    [HttpPost]
    public async Task<ActionResult<PostDto>> Create(
        [FromForm] PostDto request,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new CreatePostCommand
        {
            Body = request.Body!,
            UserId = Guid.Parse(userIdClaim),
            File = request.File!
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<PostDto>>(
            f => PostDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    public async Task<ActionResult<PostDto>> Update(
        [FromBody] PostDto request,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new UpdatePostCommand
        {
            PostId = request.Id!.Value,
            Body = request.Body!,
            UserId = Guid.Parse(userIdClaim)
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<PostDto>>(
            u => PostDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [HttpDelete("{postId:guid}")]
    public async Task<ActionResult<PostDto>> Delete(
        [FromRoute] Guid postId,
        CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        var input = new DeletePostCommand
        {
            PostId = postId,
            UserId = Guid.Parse(userIdClaim)
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<PostDto>>(
            u => PostDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }
}