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

public class ChatUsersNotFoundException(Exception innerException)
    : ChatException(ChatId.Empty(), "Users not found", innerException);
    
    public class ChatUserNotFound(ChatId chatId,UserId userId)
        : ChatException(chatId, $"User under id:{userId} into {chatId} chat not found");
        
public class ChatUserAlreadyExists(ChatId chatId,UserId userId)
    : ChatException(chatId, $"User under id:{userId} into {chatId} chat already exists");
    
    
public class ChatUserNotIntoChat(ChatId chatId,UserId userId)
    : ChatException(chatId, $"User under id:{userId} not into {chatId} chat");

public class ChatNotOwnerException(UserId userId, ChatId chatId)
    : ChatException(chatId, $"User under id:{userId} is not the owner of the chat under id: {chatId}");
    
public class ChatCannotBeEmptyException(ChatId chatId)
    : ChatException(chatId, $"Chat under id: {chatId} cannot be empty");