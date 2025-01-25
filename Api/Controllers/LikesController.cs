using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Likes.Commands;
using Domain.Likes;
using Domain.Posts;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[Route("likes")]
[ApiController]
public class LikesController(
    ISender sender,
    ILikeQueries likeQueries,
    IJwtDecoder jwtDecoder,
    ICache cache) : ControllerBase
{
    private const string CacheKeyAllLikes = "likes_all";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LikeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var cachedEntities = await cache.Get<List<LikeDto>>(CacheKeyAllLikes);
        if (cachedEntities != null)
        {
            return cachedEntities;
        }

        var entities = await likeQueries.GetAll(cancellationToken);
        var result = entities.Select(LikeDto.FromDomainModel).ToList();

        await cache.Set(CacheKeyAllLikes, result);

        return result;
    }

    [HttpGet("{likeId:guid}")]
    public async Task<ActionResult<LikeDto>> GetById(
        [FromRoute] Guid likeId,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"like_{likeId}";

        var cachedEntity = await cache.Get<LikeDto>(cacheKey);
        if (cachedEntity != null)
        {
            return cachedEntity;
        }

        var entity = await likeQueries.GetById(new LikeId(likeId), cancellationToken);

        return entity.Match<ActionResult<LikeDto>>(
            l =>
            {
                var result = LikeDto.FromDomainModel(l);

                cache.Set(cacheKey, result);

                return result;
            },
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
        var userId = Request.GetUserIdFromToken(jwtDecoder);

        var input = new CreateLikeCommand
        {
            UserId = userId,
            PostId = request.PostId!.Value
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<LikeDto>>(
            l =>
            {
                cache.Delete(CacheKeyAllLikes);
                cache.Delete($"like_{l.Id}");

                return LikeDto.FromDomainModel(l);
            },
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
            l =>
            {
                cache.Delete(CacheKeyAllLikes);
                cache.Delete($"like_{l.Id}");

                return LikeDto.FromDomainModel(l);
            },
            e => e.ToObjectResult());
    }
}