using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Chats;
using Domain.Users;
using MediatR;

namespace Application.Chats.Commands;

public record CheckUserIntoChatCommand : IRequest<Result<Chat, ChatException>>
{
    public required Guid UserId { get; init; }
    public required Guid ChatId { get; init; }
}

public class CheckUserIntoChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository) :
    IRequestHandler<CheckUserIntoChatCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(CheckUserIntoChatCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var chat = await chatRepository.GetById(new ChatId(request.ChatId), cancellationToken);
            return await chat.Match(
                async c =>
                {
                    var user = await userRepository.GetById(new UserId(request.UserId), cancellationToken);
                    return await user.Match(
                        async u => await CheckUser(c, u),
                        () => Task.FromResult<Result<Chat, ChatException>>(
                            new ChatUserNotFound(new UserId(request.UserId))));
                },
                () => Task.FromResult<Result<Chat, ChatException>>(
                    new ChatNotFoundException(new ChatId(request.ChatId))));
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(new ChatId(request.ChatId), exception);
        }
    }

    private async Task<Result<Chat, ChatException>> CheckUser(Chat chat, User user)
    {
        try
        {
            if (chat.Users.Contains(user))
                return chat;

            return new UserIntoChatNotFound(chat.Id, user.Id);
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(chat.Id, exception);
        }
    }
}