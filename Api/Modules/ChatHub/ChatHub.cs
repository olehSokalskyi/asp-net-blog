using Api.Dtos;
using Application.Chats.Commands;
using Application.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Modules.ChatHub;

public record ChatDto(Guid Id, string Name, bool IsGroup, DateTime CreatedAt)
{
    public static ChatDto FromDomainModel(Domain.Chats.Chat chat)
        => new(
            Id: chat.Id.Value,
            Name: chat.Name,
            IsGroup: chat.IsGroup,
            CreatedAt: chat.CreatedAt);
}

public record CreateChatDto(string Name, Guid OtherUserId);

[Authorize]
public class ChatHub(ISender sender, IJwtDecoder jwtDecoder) : Hub
{
    private Guid GetUserId()
    {
        var token = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = jwtDecoder.DecodeToken(token);
        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        return Guid.Parse(userIdClaim);
    }

    public async Task SendMessage(MessageDto message)
    {
        var userId = GetUserId();
        var input = new CreateMessageCommand()
        {
            Content = message.Content,
            UserId = userId,
            ChatId = message.ChatId
        };

        var result = await sender.Send(input, Context.ConnectionAborted);

        result.Match(
            async m =>
            {
                var messageDto = MessageDto.FromDomainModel(m);
                await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", messageDto);
            },
            e => Clients.Caller.SendAsync("ReceiveError", e.Message));
    }

    public async Task CreateChat(CreateChatDto chatDto)
    {
        var userId = GetUserId();
        var input = new CreateChatCommand()
        {
            Name = chatDto.Name,
            IsGroup = false,
        };

        var result = await sender.Send(input, Context.ConnectionAborted);

        result.Match(
            async chat =>
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                await Clients.Caller.SendAsync("ChatCreated", ChatDto.FromDomainModel(chat));
                var addMemberInput = new AddUserToChatCommand()
                {
                    ChatId = chat.Id.Value,
                    UserId = userId
                };
                var addResult = await sender.Send(addMemberInput, Context.ConnectionAborted);
                addResult.Match(
                    async c =>
                    {
                        var addOtherMemberInput = new AddUserToChatCommand()
                        {
                            ChatId = chat.Id.Value,
                            UserId = chatDto.OtherUserId
                        };
                        var addOtherResult = await sender.Send(addOtherMemberInput, Context.ConnectionAborted);
                        addOtherResult.Match(
                            async _ =>
                            {
                                await Clients.Group(chat.Id.ToString()).SendAsync("UserJoined", chatDto.OtherUserId);
                            },
                            e => Clients.Caller.SendAsync("ReceiveError", e.Message));
                    },
                    e => Clients.Caller.SendAsync("ReceiveError", e.Message));
            },
            e => Clients.Caller.SendAsync("ReceiveError", e.Message));
    }

    public async Task JoinChat(Guid chatId)
    {
        var userId = GetUserId();
        var input = new GetChatByIdCommand
        {
            Id = chatId
        };
        var result = await sender.Send(input, Context.ConnectionAborted);
        result.Match(
            async chat =>
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
                await Clients.Caller.SendAsync("JoinedChat", chatId);
                await Clients.Group(chatId.ToString()).SendAsync("UserJoined", userId);
            },
            e => Clients.Caller.SendAsync("ReceiveError", e.Message)
        );
    }

    //
    // public async Task CreateChat(string chatName, bool isGroup)
    // {
    //     var userId = GetUserId();
    //     var input = new CreateChatCommand()
    //     {
    //         Name = chatName,
    //         IsGroup = isGroup,
    //     };
    //
    //     var result = await sender.Send(input, Context.ConnectionAborted);
    //
    //     result.Match(
    //         async chat =>
    //         {
    //             await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
    //             await Clients.Caller.SendAsync("ChatCreated", chat.Id);
    //         },
    //         e => Clients.Caller.SendAsync("ReceiveError", e.Message));
    // }
    //
    // public async Task JoinChat(Guid chatId)
    // {
    //     await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    //     await Clients.Caller.SendAsync("JoinedChat", chatId);
    // }
}