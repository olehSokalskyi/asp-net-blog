using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Chats;
using Domain.Users;
using MediatR;

namespace Application.Chats.Commands;

public record AddUserToChatCommand : IRequest<Result<Chat, ChatException>>
{
    public required Guid ChatId { get; init; }
    public required Guid UserId { get; init; }

    public required Guid InvitorId { get; init; }
}

public class AddUserToChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository)
    : IRequestHandler<AddUserToChatCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(AddUserToChatCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var chatId = new ChatId(request.ChatId);
            var chat = await chatRepository.GetById(chatId, cancellationToken);

            return await chat.Match(
                async c =>
                {
                    var invitor = await userRepository.GetById(new UserId(request.InvitorId), cancellationToken);
                    return await invitor.Match(
                        async u =>
                        {
                            var userId = new UserId(request.UserId);
                            var user = await userRepository.GetById(userId, cancellationToken);
                            return await user.Match<Task<Result<Chat, ChatException>>>(
                                async iu =>
                                    await AddUserToChat(c, u, iu, cancellationToken),
                                () => Task.FromResult<Result<Chat, ChatException>>(
                                    new ChatUserNotFoundException(userId)
                                ));
                        },
                        () => Task.FromResult<Result<Chat, ChatException>>(
                            new ChatUserNotFoundException(new UserId(request.InvitorId))
                        ));
                },
                () => Task.FromResult<Result<Chat, ChatException>>(new ChatNotFoundException(chatId))
            );
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }

    private async Task<Result<Chat, ChatException>> AddUserToChat(
        Chat chat,
        User user,
        User invitor,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!chat.Users.Contains(invitor))
            {
                return new ChatUserNotIntoChat(chat.Id, invitor.Id);
            }

            if (chat.Users.Contains(user))
            {
                return new ChatUserAlreadyExists(chat.Id, user.Id);
            }

            chat.AddUser(user);

            return await chatRepository.Update(chat, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(chat.Id, exception);
        }
    }
}