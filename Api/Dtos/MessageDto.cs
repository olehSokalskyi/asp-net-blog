using Domain.Messages;

namespace Api.Dtos;

public record MessageDto(Guid? Id, string Content, Guid ChatId, DateTime? CreatedAt, UserDto Author)
{
    public static MessageDto FromDomainModel(Message message)
        => new(
            Id: message.Id.Value,
            Content: message.Content,
            ChatId: message.ChatId.Value,
            CreatedAt: message.CreatedAt,
            Author: UserDto.FromDomainModel(message.User)
            
            );
}