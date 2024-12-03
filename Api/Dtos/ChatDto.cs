using Domain.Chats;

namespace Api.Dtos;

public record ChatDto(Guid? Id, string? Name, List<UserDto>? Users, UserDto Owner , List<MessageDto>? Messages)
{
    public static ChatDto FromDomainModel(Chat chat)
        => new(
            Id: chat.Id.Value,
            Name: chat.Name,
            Owner: UserDto.FromDomainModel(chat.ChatOwner),
            Users: chat.Users.Select(UserDto.FromDomainModel).ToList(),
            Messages: chat.Messages.Select(MessageDto.FromDomainModel).ToList()
        );
}

public record CreateChatDto(string Name, bool IsGroup, List<Guid> UserIds);

public record UserActionChatDto(Guid ChatId, Guid UserId);
