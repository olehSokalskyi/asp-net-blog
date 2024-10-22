using Api.Dtos;
using Application.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Api.Modules.ChatHub;

public class ChatHub(ISender sender) : Hub
{
    public async Task SendMessage(MessageDto message)
    {
        var input = new CreateMessageCommand()
        {
            Content = message.Content,
            UserId = message.UserId,
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
}