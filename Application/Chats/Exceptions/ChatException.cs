using Domain.Chats;
using Domain.Users;

namespace Application.Chats.Exceptions;

public abstract class ChatException(ChatId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ChatId ChatId { get; } = id;
}

public class ChatNotFoundException(ChatId id) : ChatException(id, $"Chat under id: {id} not found");

public class ChatAlreadyExistsException(ChatId id) : ChatException(id, $"Chat already exists: {id}");

public class ChatUserNotFoundException(UserId userId)
    : ChatException(ChatId.Empty(), $"User under id:{userId} not found");

public class ChatUnknownException(ChatId id, Exception innerException)
    : ChatException(id, $"Unknown exception for the chat under id: {id}", innerException);