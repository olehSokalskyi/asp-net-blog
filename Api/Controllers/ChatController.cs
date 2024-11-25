using Api.Dtos;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Chats;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("chat")]
[ApiController]
public class ChatController(IChatRepository chatRepository, IJwtDecoder jwtDecoder, IUserRepository userRepository) : ControllerBase
{
    //TODO: Create Chat
    //TODO:Get chats by user
    //TODO:Get chatData by chatId
    
    
    // [HttpPost("create")]
    // public async Task<IActionResult<ChatDto> Create([FromBody] CreateChatDto chatDto, CancellationToken cancellationToken)
    // {
    //     
    // }
}