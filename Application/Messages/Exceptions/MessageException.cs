using Domain.Chats;
using Domain.Messages;
using Domain.Users;

namespace Application.Messages.Exceptions;

public abstract class MessageException(MessageId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public MessageId MessageId { get; } = id;
}

public class MessageNotFoundException(MessageId id) : MessageException(id, $"Message under id: {id} not found");

public class MessageUserNotFoundException(UserId userId)
    : MessageException(MessageId.Empty(), $"User under id:{userId} not found");

public class MessageChatNotFoundException(ChatId chatId)
    : MessageException(MessageId.Empty(), $"Chat under id:{chatId} not found");

public class MessageUnknownException(MessageId id, Exception innerException)
    : MessageException(id, $"Unknown exception for the message under id: {id}", innerException);