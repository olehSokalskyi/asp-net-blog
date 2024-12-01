using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Subscribers.Commands;
using Domain.Subscribers;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("subscribers")]
[ApiController]
public class SubscribersController(ISender sender, ISubscriberQueries subscriberQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SubscriberDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var entities = await subscriberQueries.GetAll(cancellationToken);

        return entities.Select(SubscriberDto.FromDomainModel).ToList();
    }
    
    [HttpGet("{subscriberId:guid}")]
    public async Task<ActionResult<SubscriberDto>> GetById(
        [FromRoute] Guid subscriberId, 
        CancellationToken cancellationToken)
    {
        var entity = await subscriberQueries.GetById(new SubscriberId(subscriberId), cancellationToken);
        return entity.Match<ActionResult<SubscriberDto>>(
            l => SubscriberDto.FromDomainModel(l),
            () => NotFound());
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IReadOnlyList<SubscriberDto>>> GetByUserId(
        [FromRoute] Guid userId, 
        CancellationToken cancellationToken)
    {
        var entities = await subscriberQueries.GetByUserId(new UserId(userId), cancellationToken);
        return entities.Select(SubscriberDto.FromDomainModel).ToList();
    }

    [HttpGet("follow/{followUserId:guid}")]
    public async Task<ActionResult<IReadOnlyList<SubscriberDto>>> GetByFollowUserId(
        [FromRoute] Guid followUserId, 
        CancellationToken cancellationToken)
    {
        var entities = await subscriberQueries.GetByFollowUserId(new UserId(followUserId), cancellationToken);
        return entities.Select(SubscriberDto.FromDomainModel).ToList();
    }
    
    [HttpPost]
    public async Task<ActionResult<SubscriberDto>> Create(
        [FromBody] SubscriberDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateSubscriberCommand
        {
            UserId = request.UserId,
            FollowUserId = request.FollowUserId.Value
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<SubscriberDto>>(
            s => SubscriberDto.FromDomainModel(s),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{subscriberId:guid}")]
    public async Task<ActionResult<SubscriberDto>> Delete(
        [FromRoute] Guid subscriberId, 
        CancellationToken cancellationToken)
    {
        var input = new DeleteSubscriberCommand()
        {
            SubscriberId = subscriberId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<SubscriberDto>>(
            s => SubscriberDto.FromDomainModel(s),
            e => e.ToObjectResult());
    }
}