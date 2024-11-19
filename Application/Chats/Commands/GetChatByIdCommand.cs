using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Chats;
using MediatR;

namespace Application.Chats.Commands;

public record GetChatByIdCommand : IRequest<Result<Chat, ChatException>>
{
    public required Guid Id { get; init; }
}

public class GetChatByIdCommandHandler(IChatRepository chatRepository)
    : IRequestHandler<GetChatByIdCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(GetChatByIdCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var chatId = new ChatId(request.Id);
            var chat = await chatRepository.GetById(chatId, cancellationToken);

            return await chat.Match(
                c => Task.FromResult<Result<Chat, ChatException>>(c),
                () => Task.FromResult<Result<Chat, ChatException>>(new ChatNotFoundException(chatId)));
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }
}