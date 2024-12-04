using Domain.Chats;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IChatRepository
{
    Task<Option<Chat>> GetById(ChatId id, CancellationToken cancellationToken);
    
    Task<Chat> Add(Chat chat, CancellationToken cancellationToken);
    Task<Option<Chat>> GetByName(string name, CancellationToken cancellationToken);
    Task<Chat> Update(Chat chat, CancellationToken cancellationToken);
}