using Api.Dtos;
using Application.Chats.Commands;
using Application.Common.Interfaces;
using Application.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Modules.ChatHub;

public class ChatHub(ISender sender, IJwtDecoder jwtDecoder) : Hub
{
    //TODO: JoinChat
    //TODO: SendMessage
    //TODO: 

    public async Task CreateChat(CreateChatDto chatDto)
    {
        var token = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var input = new CreateChatCommand()
        {
            Name = chatDto.Name,
            IsGroup = chatDto.IsGroup,
            Users = chatDto.UserIds,
            ChatOwnerId = Guid.Parse(userIdClaim)
        };
        var result = await sender.Send(input, Context.ConnectionAborted);
        result.Match(
            chat => Clients.Users(chat.Users.Select(u => u.Id.ToString()).ToList())
                .SendAsync("ChatCreated", ChatDto.FromDomainModel(chat)),
            e => Clients.Caller.SendAsync("ChatCreationFailed", e.Message));
    }

    public async Task AddUserToChat(UserActionChatDto chatDto)
    {
        var token = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var input = new AddUserToChatCommand()
        {
            ChatId = chatDto.ChatId,
            InvitorId = Guid.Parse(userIdClaim),
            UserId = chatDto.UserId
        };

        var result = await sender.Send(input, Context.ConnectionAborted);
        result.Match(
            chat => Clients.Users(chat.Users.Select(u => u.Id.ToString()).ToList())
                .SendAsync("UserAddedToChat", ChatDto.FromDomainModel(chat)),
            e => Clients.Caller.SendAsync("UserAdditionFailed", e.Message));
    }

    public async Task DeleteUserFromChat(UserActionChatDto chatDto)
    {
        var token = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var input = new DeleteUserFromChatCommand()
        {
            ChatId = chatDto.ChatId,
            OwnerId = Guid.Parse(userIdClaim),
            UserId = chatDto.UserId
        };

        var result = await sender.Send(input, Context.ConnectionAborted);
        result.Match(
            chat => Clients.Users(chat.Users.Select(u => u.Id.ToString()).ToList())
                .SendAsync("UserDeletedFromChat", ChatDto.FromDomainModel(chat)),
            e => Clients.Caller.SendAsync("UserDeletionFailed", e.Message));
    }

    public async Task SendMassage(MessageDto messageDto)
    {
        var token = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var input = new CreateMessageCommand()
        {
            ChatId = messageDto.ChatId,
            UserId = Guid.Parse(userIdClaim),
            Content = messageDto.Content
        };

        var result = await sender.Send(input, Context.ConnectionAborted);
        result.Match(
            async message =>
            {
                var chat = new GetChatByIdCommand()
                {
                    Id = messageDto.ChatId
                };
                var chatResult = await sender.Send(chat, Context.ConnectionAborted);
                chatResult.Match(
                    c => Clients.Users(c.Users.Select(u => u.Id.ToString()).ToList())
                        .SendAsync("MessageSent", MessageDto.FromDomainModel(message)),
                    e => Clients.Caller.SendAsync("MessageSendingFailed", e.Message));
            },
            e => Clients.Caller.SendAsync("MessageSendingFailed", e.Message));
    }

    public async Task SetConnectionToChat(Guid chatId)
    {
        var token = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var input = new ConnectToChatCommand()
        {
            ChatId = chatId,
            UserId = Guid.Parse(userIdClaim)
        };

        var result = await sender.Send(input, Context.ConnectionAborted);

        result.Match(
            chat => Clients.Users(chat.Users.Select(u => u.Id.ToString()).ToList())
                .SendAsync("ConnectedToChat", ChatDto.FromDomainModel(chat)),
            e => Clients.Caller.SendAsync("ConnectionToChatFailed", e.Message));
    }
}