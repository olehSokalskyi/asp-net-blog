using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Chats;
using Domain.Users;
using MediatR;

namespace Application.Chats.Commands;

public record DeleteUserFromChatCommand : IRequest<Result<Chat, ChatException>>
{
    public required Guid ChatId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid OwnerId { get; init; }
}

public class DeleteUserFromChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository) :
    IRequestHandler<DeleteUserFromChatCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(DeleteUserFromChatCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = new UserId(request.UserId);
            var chatId = new ChatId(request.ChatId);
            var chat = await chatRepository.GetById(chatId, cancellationToken);


            return await chat.Match(
                async c =>
                {
                    var owner = await userRepository.GetById(new UserId(request.OwnerId), cancellationToken);
                    return await owner.Match(
                        async u =>
                        {
                            var user = await userRepository.GetById(userId, cancellationToken);
                            return await user.Match<Task<Result<Chat, ChatException>>>(
                                async iu =>
                                    await DeleteUserFromChat(c, u, iu, cancellationToken),
                                () => Task.FromResult<Result<Chat, ChatException>>(
                                    new ChatUserNotFoundException(userId))
                            );
                        },
                        () => Task.FromResult<Result<Chat, ChatException>>(
                            new ChatNotOwnerException(new UserId(request.OwnerId), c.Id))
                    );
                },
                () => Task.FromResult<Result<Chat, ChatException>>(new ChatNotFoundException(chatId))
            );
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }

    private async Task<Result<Chat, ChatException>> DeleteUserFromChat(
        Chat chat,
        User user,
        User owner,
        CancellationToken cancellationToken)
    {
        if (chat.ChatOwnerId != owner.Id)
        {
            return new ChatNotOwnerException(owner.Id, chat.Id);
        }

        if (chat.Users.All(u => u.Id != user.Id))
        {
            return new ChatUserNotFoundException(user.Id);
        }

        if (chat.Users.Count == 1)
        {
            return new ChatCannotBeEmptyException(chat.Id);
        }

        chat.Users.Remove(user);

        return await chatRepository.Update(chat, cancellationToken);
    }
}