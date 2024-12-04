using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Posts.Commands;
using Domain.Posts;
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
    IJwtDecoder jwtDecoder,
    ICache cache) : ControllerBase
{
    private const string CacheKeyAllPosts = "posts_all";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostDto>>> GetAll(CancellationToken cancellationToken)
    {
        var cachedEntities = await cache.Get<List<PostDto>>(CacheKeyAllPosts);
        if (cachedEntities != null)
        {
            return cachedEntities;
        }

        var entities = await postQueries.GetAll(cancellationToken);
        var result = entities.Select(PostDto.FromDomainModel).ToList();

        await cache.Set(CacheKeyAllPosts, result);

        return result;
    }

    [HttpGet("{postId:guid}")]
    public async Task<ActionResult<PostDto>> GetById(
        [FromRoute] Guid postId,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"post_{postId}";

        var cachedEntity = await cache.Get<PostDto>(cacheKey);
        if (cachedEntity != null)
        {
            return cachedEntity;
        }

        var entity = await postQueries.GetById(new PostId(postId), cancellationToken);

        return entity.Match<ActionResult<PostDto>>(
            p =>
            {
                var result = PostDto.FromDomainModel(p);

                cache.Set(CacheKeyAllPosts, result);

                return result;
            },
            () => NotFound());
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
            p =>
            {
                cache.Delete(CacheKeyAllPosts);
                cache.Delete($"post_{p.Id}");

                return PostDto.FromDomainModel(p);
            },
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
            p =>
            {
                cache.Delete(CacheKeyAllPosts);
                cache.Delete($"post_{p.Id}");

                return PostDto.FromDomainModel(p);
            },
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
            p =>
            {
                cache.Delete(CacheKeyAllPosts);
                cache.Delete($"post_{p.Id}");

                return PostDto.FromDomainModel(p);
            },
            e => e.ToObjectResult());
    }
}