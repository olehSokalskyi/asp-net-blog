using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Chats;
using Domain.Users;
using MediatR;

namespace Application.Chats.Commands;

public record ConnectToChatCommand: IRequest<Result<Chat,ChatException>>
{
    public Guid ChatId { get; init; }
    public Guid UserId { get; init; }
}

public class ConnectToChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository): IRequestHandler<ConnectToChatCommand, Result<Chat,ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(ConnectToChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await chatRepository.GetById(new ChatId(request.ChatId), cancellationToken);
        return await chat.Match(
            async c =>
            {
                var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
                return await user.Match(
                    async u => await CheckUserInChat(c, u),
                    () => Task.FromResult<Result<Chat, ChatException>>(new ChatUserNotFound(new UserId(request.UserId)))
                );
            },
            () => Task.FromResult<Result<Chat, ChatException>>(new ChatNotFoundException(new ChatId(request.ChatId))));
    }
    private async Task<Result<Chat,ChatException>> CheckUserInChat(Chat chat, User user)
    {
        if (!chat.Users.Contains(user))
        {
            return new ChatUserNotIntoChat(chat.Id, user.Id);
        }

        return chat;
    }
}
