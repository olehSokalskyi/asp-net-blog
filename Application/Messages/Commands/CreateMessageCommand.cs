using Application.Common;
using Application.Common.Interfaces;
using Application.Messages.Exceptions;
using Domain.Chats;
using Domain.Messages;
using Domain.Users;
using MediatR;

namespace Application.Messages.Commands;

public record CreateMessageCommand : IRequest<Result<Message, MessageException>>
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }
    public string Content { get; init; }
}

public class CreateMessageCommandHandler(
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    IChatRepository chatRepository)
    : IRequestHandler<CreateMessageCommand, Result<Message, MessageException>>
{
    public async Task<Result<Message, MessageException>> Handle(CreateMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = new UserId(request.UserId);
            var chatId = new ChatId(request.ChatId);
            
            var user = await userRepository.GetById(userId, cancellationToken);
            var chat = await chatRepository.GetById(chatId, cancellationToken);
            
            return await user.Match(
                async u => await chat.Match(
                    async c => await CreateEntity(u.Id, c.Id, request.Content, cancellationToken),
                    () => Task.FromResult<Result<Message, MessageException>>(new MessageChatNotFoundException(chatId))),
                () => Task.FromResult<Result<Message, MessageException>>(new MessageUserNotFoundException(userId)));
        }
        catch (Exception exception)
        {
            return new MessageUnknownException(MessageId.Empty(), exception);
        }
    }

    private async Task<Result<Message, MessageException>> CreateEntity(
        UserId userId,
        ChatId chatId,
        string content,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Message.New(MessageId.New(), chatId, userId, content);

            return await messageRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new MessageUnknownException(MessageId.Empty(), exception);
        }
    }
}