using Api.Dtos;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Chats;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[Route("chat")]
[ApiController]
public class ChatController(IJwtDecoder jwtDecoder, IChatQueries chatQueries) : ControllerBase
{

    [Authorize]
    [HttpGet("getByUser")]
    public async Task<ActionResult<IReadOnlyList<ChatDto>>> GetChatsByUser(CancellationToken cancellationToken)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        
        var chats = await chatQueries.GetChatsByUser(new UserId(new Guid(userIdClaim)), cancellationToken);
        var chatDtos = chats.Select(chat => ChatDto.FromDomainModel(chat)).ToList();
        return Ok(chatDtos);
    }
    
  
}