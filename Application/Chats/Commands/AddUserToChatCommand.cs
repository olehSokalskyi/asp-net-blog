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
}

public class AddUserToChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository)
    : IRequestHandler<AddUserToChatCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(AddUserToChatCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = new UserId(request.UserId);
            var chatId = new ChatId(request.ChatId);
            var chat = await chatRepository.GetById(chatId, cancellationToken);
            var user = await userRepository.GetById(userId, cancellationToken);

            return await chat.Match(
                async c => await user.Match(
                    async u => await AddUserToChat(c, u, cancellationToken),
                    () => Task.FromResult<Result<Chat, ChatException>>(new ChatUserNotFoundException(userId))),
                () => Task.FromResult<Result<Chat, ChatException>>(new ChatNotFoundException(chatId)));
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }

    private async Task<Result<Chat, ChatException>> AddUserToChat(
        Chat chat,
        User user,
        CancellationToken cancellationToken)
    {
        try
        {
            chat.AddUser(user);

            return await chatRepository.Update(chat, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(chat.Id, exception);
        }
    }
}