using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces;
using Domain.Chats;
using MediatR;

namespace Application.Chats.Commands;

public record CreateChatCommand: IRequest<Result<Chat,ChatException>>
{
    public required string Name { get; init; }
    public required bool IsGroup { get; init; }
}

public class CreateChatCommandHandler(IChatRepository chatRepository)
    : IRequestHandler<CreateChatCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(CreateChatCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingChat = await chatRepository.GetByName(request.Name, cancellationToken);

            return await existingChat.Match(
                c => Task.FromResult<Result<Chat, ChatException>>(new ChatAlreadyExistsException(c.Id)),
                async () => await CreateEntity(request.Name, request.IsGroup, cancellationToken));
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }

    private async Task<Result<Chat, ChatException>> CreateEntity(
        string name,
        bool isGroup,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Chat.New(ChatId.New(), name, isGroup);

            return await chatRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }
}