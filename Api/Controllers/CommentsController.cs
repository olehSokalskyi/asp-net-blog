using Api.Dtos;
using Api.Modules.Errors;
using Application.Comments.Commands;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[Route("comments")]
[ApiController]
public class CommentsController(
    ISender sender,
    ICommentQueries commentQueries,
    IJwtDecoder jwtDecoder,
    ICache cache) : ControllerBase
{
    private const string CacheKeyAllComments = "comments_all";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CommentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var cachedEntities = await cache.Get<List<CommentDto>>(CacheKeyAllComments);
        if (cachedEntities != null)
        {
            return cachedEntities;
        }

        var entities = await commentQueries.GetAll(cancellationToken);
        var result = entities.Select(CommentDto.FromDomainModel).ToList();

        await cache.Set(CacheKeyAllComments, result);

        return result;
    }

    [HttpGet("{commentId:guid}")]
    public async Task<ActionResult<CommentDto>> GetById(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"comment_{commentId}";

        var cachedEntity = await cache.Get<CommentDto>(cacheKey);
        if (cachedEntity != null)
        {
            return cachedEntity;
        }

        var entity = await commentQueries.GetById(new CommentId(commentId), cancellationToken);

        return entity.Match<ActionResult<CommentDto>>(
            c =>
            {
                var result = CommentDto.FromDomainModel(c);

                cache.Set(cacheKey, result);

                return result;
            },
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(
        [FromBody] CommentDto request,
        CancellationToken cancellationToken)
    {
        var userId = Request.GetUserIdFromToken(jwtDecoder);

        var input = new CreateCommentCommand
        {
            Body = request.Body!,
            UserId = userId,
            PostId = request.PostId!.Value,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CommentDto>>(
            c =>
            {
                cache.Delete(CacheKeyAllComments);
                cache.Delete($"comment_{c.Id}");

                return CommentDto.FromDomainModel(c);
            },
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<CommentDto>> Update(
        [FromBody] CommentDto request,
        CancellationToken cancellationToken)
    {
        var userId = Request.GetUserIdFromToken(jwtDecoder);

        var input = new UpdateCommentCommand
        {
            CommentId = request.Id!.Value,
            Body = request.Body!,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CommentDto>>(
            c =>
            {
                cache.Delete(CacheKeyAllComments);
                cache.Delete($"comment_{c.Id}");

                return CommentDto.FromDomainModel(c);
            },
            e => e.ToObjectResult());
    }

    [HttpDelete("{commentId:guid}")]
    public async Task<ActionResult<CommentDto>> Delete(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var userId = Request.GetUserIdFromToken(jwtDecoder);

        var input = new DeleteCommentCommand
        {
            CommentId = commentId,
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CommentDto>>(
            c =>
            {
                cache.Delete(CacheKeyAllComments);
                cache.Delete($"comment_{c.Id}");

                return CommentDto.FromDomainModel(c);
            },
            e => e.ToObjectResult());
    }
}