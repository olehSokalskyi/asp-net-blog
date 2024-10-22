using Domain.Chats;
using Optional;

namespace Application.Common.Interfaces;

public interface IChatRepository
{
    Task<Chat> Add(Chat chat, CancellationToken cancellationToken);
    Task<Option<Chat>> GetByName(string name, CancellationToken cancellationToken);
    
    Task<Option<Chat>> GetById(ChatId id, CancellationToken cancellationToken);
    
    Task<Chat> Update(Chat chat, CancellationToken cancellationToken);
}