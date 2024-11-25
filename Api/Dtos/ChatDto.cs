using Domain.Chats;

namespace Api.Dtos;

public record ChatDto(Guid? Id, string? Name, List<UserDto>? UserDtos, List<MessageDto>? MessageDtos)
{
    public static ChatDto FromDomainModel(Chat chat)
        => new(
            Id: chat.Id.Value,
            Name: chat.Name,
            UserDtos: chat.Users.Select(UserDto.FromDomainModel).ToList(),
            MessageDtos: chat.Messages.Select(MessageDto.FromDomainModel).ToList()
        );
}