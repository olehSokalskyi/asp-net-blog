using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Chats;
using Domain.Users;
using MediatR;

namespace Application.Chats.Commands;

public record DeleteChatCommand : IRequest<Result<Chat, ChatException>>
{
    public required Guid ChatId { get; init; }
    public required Guid OwnerId { get; init; }
}

public class DeleteChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository)
    : IRequestHandler<DeleteChatCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(DeleteChatCommand request,
        CancellationToken cancellationToken)
    {
        var chat = await chatRepository.GetById(new ChatId(request.ChatId), cancellationToken);

        return await chat.Match(
            async c =>
            {
                var user = await userRepository.GetById(new UserId(request.OwnerId), cancellationToken);
                return await user.Match(
                    async u => await DeleteChat(c, u, cancellationToken),
                    () => Task.FromResult<Result<Chat, ChatException>>(
                        new ChatUserNotFound(new UserId(request.OwnerId)))
                );
            },
            () => Task.FromResult<Result<Chat, ChatException>>(new ChatNotFoundException(new ChatId(request.ChatId))));
    }

    private async Task<Result<Chat, ChatException>> DeleteChat(Chat chat, User user,
        CancellationToken cancellationToken)
    {
        if (chat.ChatOwnerId != user.Id)
            return new ChatNotOwnerException(user.Id, chat.Id);

        return await chatRepository.Delete(chat, cancellationToken);
    }
}