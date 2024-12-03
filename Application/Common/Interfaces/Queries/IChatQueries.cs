using Domain.Chats;
using Domain.Users;

namespace Application.Common.Interfaces.Queries;

public interface IChatQueries
{
    public Task<IReadOnlyList<Chat>> GetChatsByUser(UserId user, CancellationToken cancellationToken);
}